using System;
using System.Threading.Tasks;
using Moq;
using UsersApp.Application.Users;
using UsersApp.Application.Users.Dtos;
using UsersApp.Domain.Entities;
using UsersApp.Domain.Repositories;
using Xunit;

namespace UsersApp.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task Create_Fails_When_Email_Duplicate()
        {
            var repo = new Mock<IUserRepository>();
            repo.Setup(r => r.GetByEmailAsync("a@b.com")).ReturnsAsync(new User { Id = Guid.NewGuid(), Email = "a@b.com" });
            var svc = new UserService(repo.Object);

            var req = new CreateUserRequest { FullName = "A", Email = "a@b.com", Phone = "123", Address = "X" };
            var (ok, error, _) = await svc.CreateAsync(req);

            Assert.False(ok);
            Assert.Equal("Email is already in use", error);
        }

        [Fact]
        public async Task Create_Succeeds_When_Unique()
        {
            var repo = new Mock<IUserRepository>();
            var svc = new UserService(repo.Object);

            var req = new CreateUserRequest { FullName = "A", Email = "a@b.com", Phone = "123", Address = "X" };
            var (ok, error, user) = await svc.CreateAsync(req);

            Assert.True(ok);
            Assert.Null(error);
            Assert.NotNull(user);
            repo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
            repo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
