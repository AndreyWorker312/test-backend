using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersApp.Application.Users.Dtos;
using UsersApp.Domain.Entities;
using UsersApp.Domain.Repositories;

namespace UsersApp.Application.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<UserDto>> ListAsync(string? query = null)
        {
            var list = await _repo.ListAsync(query);
            return list.Select(MapToDto).ToList();
        }

        public async Task<UserDto?> GetAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<(bool ok, string? error, UserDto? user)> CreateAsync(CreateUserRequest req)
        {
            // Уникальность Email/Phone
            if (await _repo.GetByEmailAsync(req.Email) is not null)
                return (false, "Email is already in use", null);
            if (await _repo.GetByPhoneAsync(req.Phone) is not null)
                return (false, "Phone is already in use", null);

            var entity = new User
            {
                FullName = req.FullName.Trim(),
                Email = req.Email.Trim(),
                Phone = req.Phone.Trim(),
                Address = req.Address.Trim()
            };

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            return (true, null, MapToDto(entity));
        }

        public async Task<(bool ok, string? error, UserDto? user)> UpdateAsync(Guid id, UpdateUserRequest req)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null)
                return (false, "User not found", null);

            var byEmail = await _repo.GetByEmailAsync(req.Email);
            if (byEmail is not null && byEmail.Id != id)
                return (false, "Email is already in use", null);

            var byPhone = await _repo.GetByPhoneAsync(req.Phone);
            if (byPhone is not null && byPhone.Id != id)
                return (false, "Phone is already in use", null);

            entity.FullName = req.FullName.Trim();
            entity.Email = req.Email.Trim();
            entity.Phone = req.Phone.Trim();
            entity.Address = req.Address.Trim();

            await _repo.UpdateAsync(entity);
            await _repo.SaveChangesAsync();

            return (true, null, MapToDto(entity));
        }

        public async Task<(bool ok, string? error)> DeleteAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null) return (false, "User not found");
            await _repo.DeleteAsync(entity);
            await _repo.SaveChangesAsync();
            return (true, null);
        }

        private static UserDto MapToDto(User u) =>
            new(u.Id, u.FullName, u.Email, u.Phone, u.Address);
    }
}
