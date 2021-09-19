using TheGymDomain.Models.Common;

namespace TheGymDomain.Models
{
    public class User : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
