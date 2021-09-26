using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using TheGymDomain.Models;
using TheGymInfrastructure.Persistence.PostgreSQL.Config;

namespace TheGymInfrastructure.Persistence.PostgreSQL
{
    public class TheGymContext : DbContext
    {
        public TheGymContext()
        {

        }

        public TheGymContext(DbContextOptions<TheGymContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(
                builder => builder.Log(
                    (RelationalEventId.CommandExecuted, LogLevel.Debug)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
    }
}
