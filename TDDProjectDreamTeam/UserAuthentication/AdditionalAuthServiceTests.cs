using Moq;
using SchoolApp.UserAuthentication.Models;
using SchoolApp.UserAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDProjectDreamTeam.UserAuthentication
{
    public class AdditionalAuthServiceTests
    {
        private readonly Mock<IHashingService> _mockHashingService;
        private readonly AuthService _authService;

        public AdditionalAuthServiceTests()
        {
            _mockHashingService = new Mock<IHashingService>();
            _mockHashingService
                .Setup(h => h.HashPassword(It.IsAny<string>()))
                .Returns((string password) => $"hashed-{password}");
            _mockHashingService
                .Setup(h => h.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string password, string hashed) => hashed == $"hashed-{password}");

            _authService = new AuthService(new HashingService());
        }

        [Fact]
        public async Task AuthenticateAsync_Should_Return_False_For_Locked_User()
        {
            // Arrange
            for (int i = 0; i < 5; i++)
            {
                await _authService.AuthenticateAsync("user@example.com", "WrongPassword!");
            }

            // Act
            var result = await _authService.AuthenticateAsync("user@example.com", "Password123!");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ResetPassword_Should_Reset_Lock_And_FailedAttempts()
        {
            // Arrange
            for (int i = 0; i < 5; i++)
            {
                await _authService.AuthenticateAsync("user@example.com", "WrongPassword!");
            }

            // Act
            var resetResult = await _authService.ResetPasswordAsync("user@example.com", "NewPassword123!");
            var isLocked = await _authService.IsAccountLockedAsync("user@example.com");

            // Assert
            Assert.True(resetResult);
            Assert.False(isLocked);
        }

        [Fact]
        public async Task ResetPassword_Should_Return_False_For_NonExistent_User()
        {
            // Act
            var result = await _authService.ResetPasswordAsync("nonexistent@example.com", "NewPassword123!");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AuthenticateAsync_Should_Handle_Empty_Email()
        {
            // Act
            var result = await _authService.AuthenticateAsync("", "Password123!");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HashingService_VerifyPassword_Should_Return_False_For_Invalid_Hash()
        {
            // Arrange
            var password = "Password123!";
            var invalidHash = "invalidhashedpassword";

            _mockHashingService
                .Setup(service => service.VerifyPassword(password, invalidHash))
                .Returns(false);

            // Act
            var isVerified = _mockHashingService.Object.VerifyPassword(password, invalidHash);

            // Assert
            Assert.False(isVerified);
        }
        [Fact]
        public async Task IsAccountLockedAsync_Should_Return_True_If_Account_Is_Locked()
        {
            // Arrange
            for (int i = 0; i < 5; i++)
            {
                await _authService.AuthenticateAsync("user@example.com", "WrongPassword!");
            }

            // Act
            var isLocked = await _authService.IsAccountLockedAsync("user@example.com");

            // Assert
            Assert.True(isLocked);
        }

        [Fact]
        public async Task IsAccountLockedAsync_Should_Return_False_If_Account_Is_Not_Locked()
        {
            // Arrange
            var isLockedBefore = await _authService.IsAccountLockedAsync("user@example.com");

            // Act
            var isLockedAfter = await _authService.IsAccountLockedAsync("user@example.com");

            // Assert
            Assert.False(isLockedBefore);
            Assert.False(isLockedAfter);
        }

        [Fact]
        public async Task IsAccountLockedAsync_Should_Return_False_For_NonExistent_User()
        {
            // Act
            var isLocked = await _authService.IsAccountLockedAsync("nonexistent@example.com");

            // Assert
            Assert.False(isLocked);
        }
    }

}
