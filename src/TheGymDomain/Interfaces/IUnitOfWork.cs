using System.Security.Claims;
using TheGymDomain.Models;

namespace TheGymDomain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<User> UserRepository { get; }
        IRepository<UserRole> UserRoleRepository { get; }
        void Dispose();
        void Save();
        Task SaveAsync();
        Task SaveAsync(ClaimsPrincipal user);
        Task SaveAsync(string userId);
        Task SaveAsync(Guid userId);

    }
}
