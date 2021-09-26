using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGymApplication.ViewModels;
using TheGymDomain.Interfaces;
using TheGymDomain.Models;

namespace TheGymApplication.Controllers
{
    /// <summary>
    /// Uses Generic controller methods for CRUD, can implement custom methods for CRUD with override
    /// </summary>
    public class UserController : GenericController<User, UserViewModel>
    {
        public UserController(IUnitOfWork unitOfWork, IMapper mapper, IValidator<User> validator)
            : base(unitOfWork, mapper, unitOfWork.UserRepository, validator)
        {

        }
    }
}
