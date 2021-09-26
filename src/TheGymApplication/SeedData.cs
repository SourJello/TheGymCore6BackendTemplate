using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGymDomain.Models;
using TheGymInfrastructure.Persistence.PostgreSQL;

namespace TheGymApplication
{
    public class SeedData
    {
        public static void DevelopInitialize(TheGymContext context)
        {
            if (context.Users.Count() == 0)
            {
                SeedUsers(context);
                Log.Information("Users Seeded");
            }
            if (context.UserRoles.Count() == 0)
            {
                SeedUsers(context);
                Log.Information("Users Seeded");
            }
        }

        public static void SeedRoles(TheGymContext context)
        {
            var roles = new UserRole[]
            {
                new UserRole()
                {
                    RoleName = "Admin"
                },
                new UserRole()
                {
                    RoleName = "User"
                }
            };
            context.UserRoles.AddRange(roles);
            context.SaveChanges();
        }
        public static void SeedUsers(TheGymContext context)
        {
            var roles = new List<UserRole>() { 
                context.UserRoles.First(x => x.RoleName.Equals("Admin")),
                context.UserRoles.First(x => x.RoleName.Equals("User"))
            };
            var users = new User[]
            {
                new User()
                {
                    FirstName = "Doug",
                    LastName = "DimmaDome",
                    Phone = "123-456-7890",
                    UserRoles = roles
                },
                new User()
                {
                    FirstName = "Mike",
                    LastName = "Strongman",
                    Phone = "123-456-7899",
                    UserRoles = context.UserRoles.Where(x => x.RoleName.Equals("User")).ToList()
                },
            };
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
