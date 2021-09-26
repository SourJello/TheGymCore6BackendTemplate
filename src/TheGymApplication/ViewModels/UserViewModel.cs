using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGymApplication.ViewModels.Common;

namespace TheGymApplication.ViewModels
{
    public class UserViewModel : EntityViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    }
}
