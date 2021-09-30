using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Reflection;

//TODO: clean up logging and remove any dead code
//TODO: change logging based on environment
namespace TheGymApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigureLogging();
            CreateHost(args);
        }

        private static void ConfigureLogging()
        {


            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                .ReadFrom.Configuration(CreateConfigurataionRoot().Build())
                .CreateLogger();
        }
        private static IConfigurationBuilder CreateConfigurataionRoot()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                ;
            return configuration;
        }
        private static void CreateHost(string[] args)
        {
            try
            {
                Log.Information("Starting web host");
                var host = CreateHostBuilder(args).Build();
                InitializeDb(host);
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal($"Failed to start {Assembly.GetExecutingAssembly().GetName().Name}", ex);
                throw;
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>

            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration(configuration => 
                CreateConfigurataionRoot())
                .UseSerilog();

        private static void InitializeDb(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    Log.Information("Initializing DB");
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    var isDevelopment = environment == Environments.Development;
                    var context = services.GetRequiredService<TheGymInfrastructure.Persistence.PostgreSQL.TheGymContext>();
                    SeedData.Clean(context);
                    Log.Information($"DB Cleaned from {Assembly.GetExecutingAssembly().GetName().Name}");
                    SeedData.DevelopInitialize(context);
                    Log.Information($"DB Initialized from {Assembly.GetExecutingAssembly().GetName().Name}");

                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"An error occurred while seeding the database from {Assembly.GetExecutingAssembly().GetName().Name}.");
                }
            }
        }
    }
}
