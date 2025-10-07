using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsersApp.Domain.Entities;

namespace UsersApp.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByPhoneAsync(string phone);
        Task<List<User>> ListAsync(string? query = null);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task SaveChangesAsync();
    }
}
