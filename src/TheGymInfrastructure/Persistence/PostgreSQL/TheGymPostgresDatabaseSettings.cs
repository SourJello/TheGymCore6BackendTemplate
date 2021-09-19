using TheGymDomain.Interfaces;


namespace TheGymInfrastructure.Persistence.PostgreSQL
{
    public class TheGymPostgresDatabaseSettings : IPostgresDatabaseSettings
    {
        public string ConnectionString { get; set; }
    }
}
