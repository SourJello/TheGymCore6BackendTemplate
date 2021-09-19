using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TheGymDomain.Interfaces;
using TheGymDomain.Models;
using TheGymDomain.Models.Common;
using TheGymInfrastructure.Persistence.PostgreSQL;
using TheGymInfrastructure.Repositories;

namespace TheGymInfrastructure
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private TheGymContext _context;
        private bool disposed = false;

        private IRepository<User> _userRepository;
        private IRepository<UserRole> _userRoleRepository;

        public UnitOfWork(TheGymContext context)
        {
            _context = context;
        }

        public IRepository<User> UserRepository => _userRepository ?? 
            (_userRepository = new GenericRepository<User>(_context));
        public IRepository<UserRole> UserRoleRepository => _userRoleRepository ??
            (_userRoleRepository = new GenericRepository<UserRole>(_context));

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task SaveAsync(ClaimsPrincipal user)
        {
            var id = user.FindFirst("id");
            try
            {
                await SaveAsync(id.Value);
            }
            catch
            {
                throw;
            }
        }

        public async Task SaveAsync(string userId)
        {
            Guid id;
            if(Guid.TryParse(userId, out id))
            {
                await SaveAsync(id);
            }
            else
            {
                throw new FormatException("Unable to parse string to guid");
            }
        }

        public async Task SaveAsync(Guid userId)
        {
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                //If not null and the entity inherits from the base entity class
                if(entry.Entity != null && entry.Entity is Entity)
                {
                    if(entry.State == EntityState.Added)
                    {
                        entry.Property("CreatedByUserId").CurrentValue = userId;
                    }
                    if(entry.State == EntityState.Modified)
                    {
                        entry.Property("ModifiedByUserId").CurrentValue = userId;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public async Task DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }
        protected virtual async Task DisposeAsync(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.DisposeAsync();
                }
            }
            this.disposed = true;
        }


    }
}
