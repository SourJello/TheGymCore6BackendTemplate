using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGymApplication.ViewModels;
using TheGymDomain.Models;

namespace TheGymApplication
{
    public static class AutoMapperSetup
    {
        public static void SetupAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(x =>
            {
                x.CreateMap<User, UserViewModel>().ReverseMap();
                x.CreateMap<UserRole, UserRoleViewModel>().ReverseMap();
            });
        }
    }
}
