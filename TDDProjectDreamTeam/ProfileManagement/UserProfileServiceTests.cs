using Xunit;
using Moq;
using SchoolApp.Models;
using SchoolApp.Repositories;
using SchoolApp.ProfileManagement;
using SchoolApp.UserAuthentication;

namespace TDDProjectDreamTeam.ProfileManagement
{
    public class UserProfileServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IHashingService> _mockHashingService;
        private readonly UserProfileService _service;

        public UserProfileServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockHashingService = new Mock<IHashingService>();
            _service = new UserProfileService(_mockUserRepository.Object, _mockHashingService.Object);
        }

        [Fact]
        public void GetProfile_WithValidUserId_ShouldReturnUserProfile()
        {
            var userId = "user123";
            var user = new User(userId, "John Doe", "john@example.com", "hashedPassword", null);
            _mockUserRepository.Setup(repo => repo.GetUser(userId)).Returns(user);

            var result = _service.GetProfile(userId);

            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("John Doe", result.Name);
        }

        [Fact]
        public void GetProfile_WithUnauthorizedUserId_ShouldReturnNull()
        {
            var userId = "unauthorizedUser";
            _mockUserRepository.Setup(repo => repo.GetUser(userId)).Returns((User)null);

            var result = _service.GetProfile(userId);

            Assert.Null(result);
        }

        [Fact]
        public void UpdateProfile_WithValidData_ShouldUpdateUser()
        {
            var userId = "user123";
            var existingUser = new User(userId, "John Doe", "john@example.com", "hashedPassword", null);
            var updatedUser = new User(userId, "John Updated", "john.updated@example.com", null, null);

            _mockUserRepository.Setup(repo => repo.GetUser(userId)).Returns(existingUser);
            _mockUserRepository.Setup(repo => repo.GetUserByEmail(updatedUser.Email)).Returns((User)null);

            var result = _service.UpdateProfile(userId, updatedUser);

            Assert.True(result);
            _mockUserRepository.Verify(repo => repo.UpdateUser(It.Is<User>(u => u.Name == "John Updated" && u.Email == "john.updated@example.com")), Times.Once);
        }

        [Fact]
        public void UpdateProfile_WithInvalidEmail_ShouldReturnFalse()
        {
            var userId = "user123";
            var existingUser = new User(userId, "John Doe", "john@example.com", "hashedPassword", null);
            var updatedUser = new User(userId, "John Updated", "invalid-email", null, null);

            _mockUserRepository.Setup(repo => repo.GetUser(userId)).Returns(existingUser);

            var result = _service.UpdateProfile(userId, updatedUser);

            Assert.False(result);
        }

        [Fact]
        public void UpdateProfile_WithDuplicateEmail_ShouldReturnFalse()
        {
            var userId = "user123";
            var duplicateEmail = "duplicate@example.com";
            var existingUser = new User(userId, "John Doe", "john@example.com", "hashedPassword", null);
            var updatedUser = new User(userId, "John Updated", duplicateEmail, null, null);
            var conflictingUser = new User("user456", "Jane Doe", duplicateEmail, "hashedPassword", null);

            _mockUserRepository.Setup(repo => repo.GetUser(userId)).Returns(existingUser);
            _mockUserRepository.Setup(repo => repo.GetUserByEmail(duplicateEmail)).Returns(conflictingUser);

            var result = _service.UpdateProfile(userId, updatedUser);

            Assert.False(result);
        }

        [Fact]
        public void UpdatePassword_WithCorrectCurrentPassword_ShouldUpdatePassword()
        {
            var userId = "user123";
            var currentPassword = "OldPassword123";
            var newPassword = "NewPassword123!";
            var user = new User(userId, "John Doe", "john@example.com", "hashedOldPassword", null);

            _mockUserRepository.Setup(repo => repo.GetUser(userId)).Returns(user);
            _mockHashingService.Setup(h => h.VerifyPassword(currentPassword, user.PasswordHash)).Returns(true);
            _mockHashingService.Setup(h => h.HashPassword(newPassword)).Returns("hashedNewPassword");

            var result = _service.UpdatePassword(userId, currentPassword, newPassword);

            Assert.True(result);
            _mockUserRepository.Verify(repo => repo.UpdateUser(It.Is<User>(u => u.PasswordHash == "hashedNewPassword")), Times.Once);
        }

        [Fact]
        public void UpdatePassword_WithIncorrectCurrentPassword_ShouldReturnFalse()
        {
            var userId = "user123";
            var currentPassword = "WrongPassword";
            var newPassword = "NewPassword123!";
            var user = new User(userId, "John Doe", "john@example.com", "hashedOldPassword", null);

            _mockUserRepository.Setup(repo => repo.GetUser(userId)).Returns(user);
            _mockHashingService.Setup(h => h.VerifyPassword(currentPassword, user.PasswordHash)).Returns(false);

            var result = _service.UpdatePassword(userId, currentPassword, newPassword);

            Assert.False(result);
        }

        [Fact]
        public void UpdatePassword_WithWeakNewPassword_ShouldReturnFalse()
        {
            var userId = "user123";
            var currentPassword = "CorrectPassword";
            var weakPassword = "123";
            var user = new User(userId, "John Doe", "john@example.com", "hashedCorrectPassword", null);

            _mockUserRepository.Setup(repo => repo.GetUser(userId)).Returns(user);
            _mockHashingService.Setup(h => h.VerifyPassword(currentPassword, user.PasswordHash)).Returns(true);

            var result = _service.UpdatePassword(userId, currentPassword, weakPassword);

            Assert.False(result);
        }

        [Fact]
        public void IsAuthorizedToUpdateProfile_ByAdmin_ShouldReturnTrue()
        {
            var userId = "admin123";
            var profileId = "user123";
            var adminRole = new Role("Admin", new List<string> { "ManageUsers" });
            var adminUser = new User(userId, "Admin User", "admin@example.com", "hashedPassword", adminRole);

            _mockUserRepository.Setup(repo => repo.GetUser(userId)).Returns(adminUser);

            var result = _service.IsAuthorizedToUpdateProfile(userId, profileId);

            Assert.True(result);
        }

        [Fact]
        public void IsAuthorizedToUpdateProfile_ByNonAdmin_ShouldReturnFalse()
        {
            var userId = "user123";
            var profileId = "anotherUser";
            var userRole = new Role("User", new List<string>());
            var user = new User(userId, "John Doe", "john@example.com", "hashedPassword", userRole);

            _mockUserRepository.Setup(repo => repo.GetUser(userId)).Returns(user);

            var result = _service.IsAuthorizedToUpdateProfile(userId, profileId);

            Assert.False(result);
        }

        [Fact]
        public void UpdateProfile_SimultaneousUpdates_ShouldHandleGracefully()
        {
            var userId = "user123";
            var initialUser = new User(userId, "Initial Name", "initial@example.com", "hashedPassword", null);

            _mockUserRepository.Setup(repo => repo.GetUser(userId)).Returns(initialUser);

            var update1 = new User(userId, "Updated Name 1", null, null, null);
            var update2 = new User(userId, null, "updated2@example.com", null, null);

            Parallel.Invoke(
                () => _service.UpdateProfile(userId, update1),
                () => _service.UpdateProfile(userId, update2)
            );

            _mockUserRepository.Verify(repo => repo.UpdateUser(It.IsAny<User>()), Times.AtLeastOnce);
        }
    }
}
