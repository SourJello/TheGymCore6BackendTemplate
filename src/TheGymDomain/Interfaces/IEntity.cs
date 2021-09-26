namespace TheGymDomain.Interfaces
{
    /// <summary>
    /// Not really need right now but to be used
    /// in the future when communication between
    /// microservices is needed, at that point
    /// it would be in an external shared library
    /// </summary>
    public interface IEntity
    {
        Guid Id { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid CreatedByUserId { get; set; }
        Guid ModifiedByUserId { get; set; }
        bool IsDeleted { get; set; }
    }
}
