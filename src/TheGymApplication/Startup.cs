using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;
using TheGymDomain.Interfaces;
using TheGymDomain.Models.Common;
using TheGymInfrastructure;
using TheGymInfrastructure.Persistence.PostgreSQL;
using TheGymInfrastructure.Persistence.PostgreSQL.Config.Common;

namespace TheGymApplication
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                    "TheGymOpenAPISpecification",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "The Gym Api",
                        Version = "1"
                    });
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                setupAction.IncludeXmlComments(xmlCommentsFullPath);
            });

            //Postgres configuration
            services.Configure<TheGymPostgresDatabaseSettings>(
                Configuration.GetSection(nameof(TheGymPostgresDatabaseSettings)));
            StartupSetup.AddPostgresContext(services, Configuration.GetConnectionString("DefaultConnection"));

            //Unit Of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Newtonsoft
            services.AddControllers().AddNewtonsoftJson();
            //Automapper
            AutoMapperSetup.SetupAutoMapper(services);

            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            services.AddValidatorsFromAssemblyContaining<EntityValidator<Entity>>(ServiceLifetime.Transient);

            //TODO: Add Authorization and Authentication
            //TODO: create db with a docker file, add migrations, update db, and run this bad boy
            //TODO: Logging
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //TODO: Add Authorization and Authentication
            //TODO: https redirection

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(
                    "swagger/TheGymOpenApiSpecification/swagger.json",
                    "The Gym Api");
                setupAction.RoutePrefix = "";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
