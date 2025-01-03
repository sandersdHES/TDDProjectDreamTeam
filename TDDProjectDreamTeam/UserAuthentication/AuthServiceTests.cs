using Xunit;
using Moq;
using SchoolApp.UserAuthentication;

namespace TDDProjectDreamTeam.UserAuthentication
{
    public class AuthServiceTests
    {
        private readonly IAuthService _authService;
        private readonly Mock<IHashingService> _mockHashingService;

        public AuthServiceTests()
        {
            _mockHashingService = new Mock<IHashingService>();
            _mockHashingService.Setup(h => h.HashPassword(It.IsAny<string>())).Returns((string password) => $"hashed-{password}");
            _mockHashingService.Setup(h => h.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string password, string hashed) => hashed == $"hashed-{password}");

            _authService = new AuthService(_mockHashingService.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_Should_Return_True_For_Valid_Credentials()
        {
            // Arrange
            var email = "user@example.com";
            var password = "Password123!";

            // Act
            var result = await _authService.AuthenticateAsync(email, password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AuthenticateAsync_Should_Return_False_For_Invalid_Credentials()
        {
            // Arrange
            var email = "user@example.com";
            var password = "WrongPassword!";

            // Act
            var result = await _authService.AuthenticateAsync(email, password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ResetPassword_Should_Reset_User_Password_Successfully()
        {
            // Arrange
            var email = "user@example.com";
            var newPassword = "NewPassword123!";

            // Act
            var resetResult = await _authService.ResetPasswordAsync(email, newPassword);
            var authResult = await _authService.AuthenticateAsync(email, newPassword);

            // Assert
            Assert.True(resetResult);
            Assert.True(authResult);
        }

        [Fact]
        public async Task ResetPassword_Should_Return_False_For_Nonexistent_User()
        {
            // Arrange
            var email = "nonexistent@example.com";
            var newPassword = "NewPassword123!";

            // Act
            var result = await _authService.ResetPasswordAsync(email, newPassword);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AuthenticateAsync_Should_Handle_Account_Lockout_After_Failed_Attempts()
        {
            // Arrange
            var email = "user@example.com";
            var wrongPassword = "WrongPassword!";
            const int failedAttempts = 5;

            // Act
            for (int i = 0; i < failedAttempts; i++)
            {
                await _authService.AuthenticateAsync(email, wrongPassword);
            }

            var result = await _authService.AuthenticateAsync(email, "Password123!");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task IsAccountLockedAsync_Should_Return_True_When_Account_Is_Locked()
        {
            // Arrange
            var email = "user@example.com";
            var wrongPassword = "WrongPassword!";
            const int failedAttempts = 5;

            for (int i = 0; i < failedAttempts; i++)
            {
                await _authService.AuthenticateAsync(email, wrongPassword);
            }

            // Act
            var isLocked = await _authService.IsAccountLockedAsync(email);

            // Assert
            Assert.True(isLocked);
        }

        [Fact]
        public async Task IsAccountLockedAsync_Should_Return_False_For_Nonexistent_User()
        {
            // Act
            var isLocked = await _authService.IsAccountLockedAsync("nonexistent@example.com");

            // Assert
            Assert.False(isLocked);
        }
    }
}
