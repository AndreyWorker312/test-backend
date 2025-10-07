using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UsersApp.Domain.Entities;
using UsersApp.Domain.Repositories;
using UsersApp.Infrastructure.Data;

namespace UsersApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersDbContext _db;

        public UserRepository(UsersDbContext db) => _db = db;

        public Task<User?> GetByIdAsync(Guid id) =>
            _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public Task<User?> GetByEmailAsync(string email) =>
            _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

        public Task<User?> GetByPhoneAsync(string phone) =>
            _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Phone == phone);

        public async Task<List<User>> ListAsync(string? query = null)
        {
            IQueryable<User> q = _db.Users.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(query))
            {
                var term = query.Trim();
                q = q.Where(x => x.FullName.Contains(term) || x.Email.Contains(term));
            }
            return await q.OrderBy(x => x.FullName).ToListAsync();
        }

        public Task AddAsync(User user)
        {
            _db.Users.Add(user);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(User user)
        {
            _db.Users.Update(user);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(User user)
        {
            _db.Users.Remove(user);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync() => _db.SaveChangesAsync();
    }
}
