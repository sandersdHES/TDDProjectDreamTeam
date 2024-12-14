using System;
using Xunit;
using Moq;
using SchoolApp;
using SchoolApp.Models;
using SchoolApp.UserAuthentication;
using SchoolApp.UserRegistration;
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
                _mockUserRegistrationService.Object
            );
        }

        [Fact]
        public void RegisterUser_NullUser_ThrowsArgumentNullException()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _userService.RegisterUser(null, "password"));
        }

        [Fact]
        public void RegisterUser_InvalidRegistrationDetails_ThrowsArgumentException()
        {
            // Arrange
            var user = new User("1", "Test User", "test@example.com", null, new Role("User", new List<string>()));
            _mockUserRegistrationService.Setup(x => x.RegisterUser(user.Name, user.Email, "password")).Returns(false);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _userService.RegisterUser(user, "password"));
        }

        [Fact]
        public void RegisterUser_ValidUser_ReturnsRegisteredUser()
        {
            // Arrange
            var user = new User("1", "Test User", "test@example.com", null, new Role("User", new List<string>()));
            _mockUserRegistrationService.Setup(x => x.RegisterUser(user.Name, user.Email, "password")).Returns(true);
            _mockHashingService.Setup(x => x.HashPassword("password")).Returns("hashedPassword");

            // Act
            var result = _userService.RegisterUser(user, "password");

            // Assert
            Assert.Equal(user, result);
            Assert.Equal("hashedPassword", user.PasswordHash);
            _mockUserRepository.Verify(repo => repo.AddUser(user), Times.Once);
        }

        [Fact]
        public void AuthenticateUser_EmptyCredentials_ThrowsArgumentException()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _userService.AuthenticateUser("", "password"));
            Assert.Throws<ArgumentException>(() => _userService.AuthenticateUser("test@example.com", ""));
        }

        [Fact]
        public void AuthenticateUser_InvalidEmailOrPassword_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var user = new User("1", "Test User", "test@example.com", "hashedPassword", new Role("User", new List<string>()));
            _mockUserRepository.Setup(repo => repo.GetUserByEmail("test@example.com")).Returns(user);
            _mockHashingService.Setup(x => x.VerifyPassword("password", "hashedPassword")).Returns(false);

            // Act & Assert
            Assert.Throws<UnauthorizedAccessException>(() => _userService.AuthenticateUser("test@example.com", "password"));
        }

        [Fact]
        public void AuthenticateUser_ValidCredentials_ReturnsAuthenticatedUser()
        {
            // Arrange
            var user = new User("1", "Test User", "test@example.com", "hashedPassword", new Role("User", new List<string>()));
            _mockUserRepository.Setup(repo => repo.GetUserByEmail("test@example.com")).Returns(user);
            _mockHashingService.Setup(x => x.VerifyPassword("password", "hashedPassword")).Returns(true);

            // Act
            var result = _userService.AuthenticateUser("test@example.com", "password");

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public void GetUserById_UserFound_ReturnsUser()
        {
            // Arrange
            var user = new User("1", "Test User", "test@example.com", "hashedPassword", new Role("User", new List<string>()));
            _mockUserRepository.Setup(repo => repo.GetUser("1")).Returns(user);

            // Act
            var result = _userService.GetUserById("1");

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public void GetUserById_InvalidUserId_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetUser(It.IsAny<string>())).Throws<KeyNotFoundException>();

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _userService.GetUserById(null));
            Assert.Throws<KeyNotFoundException>(() => _userService.GetUserById(""));
            Assert.Throws<KeyNotFoundException>(() => _userService.GetUserById("invalid"));
        }
    }
}
