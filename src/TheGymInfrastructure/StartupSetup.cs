
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TheGymInfrastructure.Persistence.PostgreSQL;

namespace TheGymInfrastructure
{
    public static class StartupSetup
    {
        public static void AddPostgresContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<TheGymContext>(options =>
                options.UseNpgsql(connectionString, i => i.MigrationsAssembly("TheGymApplication"))
                .EnableSensitiveDataLogging(true)
                .EnableDetailedErrors(true)
                .UseLazyLoadingProxies()
                );
        }
    }
}
