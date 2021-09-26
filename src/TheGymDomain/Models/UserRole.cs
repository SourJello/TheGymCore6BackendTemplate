using TheGymDomain.Models.Common;

namespace TheGymDomain.Models
{
    public class UserRole : Entity
    {
        public string RoleName { get; set; }
        public virtual ICollection<User> Users { get; set; }

    }
}
