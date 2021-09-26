using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TheGymApplication.Filters;
using TheGymDomain.Interfaces;
using TheGymInfrastructure;
using TheGymInfrastructure.Persistence.PostgreSQL;

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
            services.AddControllers(options =>
                {
                    options.Filters.Add(typeof(ValidatorActionFilter));
                })
                .AddNewtonsoftJson();
            //Automapper
            AutoMapperSetup.SetupAutoMapper(services);

            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });
            //TODO: Add Authorization and Authentication
            //TODO: Add filter to check modelstate validation
            //TODO: create db with a docker file, add migrations, update db, and run this bad boy
            //TODO: Add entity validation using fluidvalidator
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
