using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsersApp.Application.Users.Dtos;

namespace UsersApp.Application.Users
{
    public interface IUserService
    {
        Task<List<UserDto>> ListAsync(string? query = null);
        Task<UserDto?> GetAsync(Guid id);
        Task<(bool ok, string? error, UserDto? user)> CreateAsync(CreateUserRequest req);
        Task<(bool ok, string? error, UserDto? user)> UpdateAsync(Guid id, UpdateUserRequest req);
        Task<(bool ok, string? error)> DeleteAsync(Guid id);
    }
}
