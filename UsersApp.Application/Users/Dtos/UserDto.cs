using System;

namespace UsersApp.Application.Users.Dtos
{
    public record UserDto(Guid Id, string FullName, string Email, string Phone, string Address);
}
