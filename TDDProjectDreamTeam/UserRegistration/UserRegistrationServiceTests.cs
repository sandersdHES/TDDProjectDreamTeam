using Xunit;
using Moq;
using SchoolApp.UserRegistration.Models;
using SchoolApp.UserRegistration;

namespace TDDProjectDreamTeam.UserRegistration
{
    public class UserRegistrationServiceTests
    {
        private readonly UserRegistrationService _userRegistrationService;

        public UserRegistrationServiceTests()
        {
            _userRegistrationService = new UserRegistrationService();
        }

        [Fact]
        public void RegisterUser_Should_Succeed_With_Valid_Inputs()
        {
            // Arrange
            var name = "John Doe";
            var email = "john.doe@example.com";
            var password = "StrongPassword123!";

            // Act
            var result = _userRegistrationService.RegisterUser(name, email, password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void RegisterUser_Should_Fail_With_Empty_Inputs()
        {
            // Arrange
            var name = "";
            var email = "";
            var password = "";

            // Act
            var result = _userRegistrationService.RegisterUser(name, email, password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Fail_With_Invalid_Email_Format()
        {
            // Arrange
            var name = "John Doe";
            var email = "invalid-email";
            var password = "StrongPassword123!";

            // Act
            var result = _userRegistrationService.RegisterUser(name, email, password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Fail_With_Weak_Password()
        {
            // Arrange
            var name = "John Doe";
            var email = "john.doe@example.com";
            var password = "weak";

            // Act
            var result = _userRegistrationService.RegisterUser(name, email, password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Add_Email_To_RegisteredEmails()
        {
            // Arrange
            var name = "John Doe";
            var email = "new.user@example.com";
            var password = "StrongPassword123!";

            // Act
            var result = _userRegistrationService.RegisterUser(name, email, password);

            // Assert
            Assert.True(result);
            Assert.False(_userRegistrationService.IsEmailAvailable(email));
        }

        [Fact]
        public void ValidateName_Should_Fail_If_Name_Is_Too_Short()
        {
            // Arrange
            var shortName = "A";

            // Act
            var result = _userRegistrationService.ValidateName(shortName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateName_Should_Succeed_With_Valid_Name()
        {
            // Arrange
            var validName = "John Doe";

            // Act
            var result = _userRegistrationService.ValidateName(validName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidEmail_Should_Succeed_With_Valid_Email()
        {
            // Arrange
            var email = "user@example.com";

            // Act
            var result = _userRegistrationService.IsValidEmail(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsEmailAvailable_Should_Fail_If_Email_Exists()
        {
            // Arrange
            var email = "existing.email@example.com";

            // Act
            var result = _userRegistrationService.IsEmailAvailable(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidPassword_Should_Succeed_With_Strong_Password()
        {
            // Arrange
            var password = "Strong@Password123";

            // Act
            var result = _userRegistrationService.IsValidPassword(password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void RegisterUser_Should_Prevent_SQL_Injection()
        {
            // Arrange
            var maliciousInput = "'; DROP TABLE Users; --";

            // Act
            var result = _userRegistrationService.RegisterUser(maliciousInput, maliciousInput, maliciousInput);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Handle_Email_Case_Insensitivity()
        {
            // Arrange
            var emailUpper = "USER@EXAMPLE.COM";
            var emailLower = "user@example.com";

            // Act
            var isAvailableUpper = _userRegistrationService.IsEmailAvailable(emailUpper);
            var isAvailableLower = _userRegistrationService.IsEmailAvailable(emailLower);

            // Assert
            Assert.True(isAvailableUpper);
            Assert.True(isAvailableLower);
        }
    }
}
