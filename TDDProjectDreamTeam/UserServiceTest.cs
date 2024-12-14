using System;
using Xunit;
using Moq;
using SchoolApp;
using SchoolApp.Models;
using SchoolApp.UserAuthentication;
using SchoolApp.UserRegistration;
using System.Collections.Generic;
using SchoolApp.Repositories;

namespace TDDProjectDreamTeam
{
    public class UserServiceTest
    {
        private readonly Mock<IHashingService> _mockHashingService;
        private readonly Mock<IUserRegistrationService> _mockUserRegistrationService;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _mockHashingService = new Mock<IHashingService>();
            _mockUserRegistrationService = new Mock<IUserRegistrationService>();
            _mockUserRepository = new Mock<IUserRepository>();

            _userService = new UserService(
                _mockUserRepository.Object,
                _mockHashingService.Object,
                _mockUserRegistrationService.Object,
                message => { }
            );
        }

        [Fact]
        public void RegisterUser_NullUser_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _userService.RegisterUser(null, "password"));
        }

        [Fact]
        public void RegisterUser_InvalidRegistrationDetails_ThrowsArgumentException()
        {
            var user = new User("1", "Test User", "test@example.com", null, new Role("User", new List<string>()));
            _mockUserRegistrationService.Setup(x => x.RegisterUser(user.Name, user.Email, "password")).Returns(false);

            Assert.Throws<ArgumentException>(() => _userService.RegisterUser(user, "password"));
        }

        [Fact]
        public void RegisterUser_ValidUser_ReturnsRegisteredUser()
        {
            var user = new User("1", "Test User", "test@example.com", null, new Role("User", new List<string>()));
            _mockUserRegistrationService.Setup(x => x.RegisterUser(user.Name, user.Email, "password")).Returns(true);
            _mockHashingService.Setup(x => x.HashPassword("password")).Returns("hashedPassword");

            var result = _userService.RegisterUser(user, "password");

            Assert.Equal(user, result);
            Assert.Equal("hashedPassword", user.PasswordHash);
            _mockUserRepository.Verify(repo => repo.AddUser(user), Times.Once);
        }

        [Fact]
        public void AuthenticateUser_EmptyEmail_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _userService.AuthenticateUser("", "password"));
        }

        [Fact]
        public void AuthenticateUser_EmptyPassword_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _userService.AuthenticateUser("test@example.com", ""));
        }

        [Fact]
        public void AuthenticateUser_InvalidEmailOrPassword_ThrowsUnauthorizedAccessException()
        {
            var user = new User("1", "Test User", "test@example.com", "hashedPassword", new Role("User", new List<string>()));
            _mockUserRepository.Setup(repo => repo.GetUserByEmail("test@example.com")).Returns(user);
            _mockHashingService.Setup(x => x.VerifyPassword("password", "hashedPassword")).Returns(false);

            Assert.Throws<UnauthorizedAccessException>(() => _userService.AuthenticateUser("test@example.com", "password"));
        }

        [Fact]
        public void AuthenticateUser_ValidCredentials_ReturnsAuthenticatedUser()
        {
            var user = new User("1", "Test User", "test@example.com", "hashedPassword", new Role("User", new List<string>()));
            _mockUserRepository.Setup(repo => repo.GetUserByEmail("test@example.com")).Returns(user);
            _mockHashingService.Setup(x => x.VerifyPassword("password", "hashedPassword")).Returns(true);

            var result = _userService.AuthenticateUser("test@example.com", "password");

            Assert.Equal(user, result);
        }

        [Fact]
        public void GetUserById_UserNotFound_ThrowsKeyNotFoundException()
        {
            _mockUserRepository.Setup(repo => repo.GetUser("1")).Throws(new KeyNotFoundException());

            Assert.Throws<KeyNotFoundException>(() => _userService.GetUserById("1"));
        }

        [Fact]
        public void GetUserById_UserFound_ReturnsUser()
        {
            var user = new User("1", "Test User", "test@example.com", "hashedPassword", new Role("User", new List<string>()));
            _mockUserRepository.Setup(repo => repo.GetUser("1")).Returns(user);

            var result = _userService.GetUserById("1");

            Assert.Equal(user, result);
        }
    }
}
