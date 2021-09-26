using TheGymDomain.Interfaces;

namespace TheGymDomain.Models.Common
{
    public abstract class Entity : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid ModifiedByUserId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
