//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;

//namespace TheGymInfrastructure.Persistence.PostgreSQL
//{
//    class TheGymContextFactory : IDesignTimeDbContextFactory<TheGymContext>
//    {
//        public TheGymContext CreateDbContext(string[] args)
//        {
//            //TODO: non hardcoded refernce to the connection string
//            var optionBuilder = new DbContextOptionsBuilder<TheGymContext>();
//            optionBuilder.UseNpgsql("User ID=user;Password=password;Port=5432;Server=127.0.0.1;Database=postgres;Pooling=true;");
//            return new TheGymContext(optionBuilder.Options);
//        }
//    }
//}
