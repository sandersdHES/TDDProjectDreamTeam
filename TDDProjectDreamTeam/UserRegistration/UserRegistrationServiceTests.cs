using Xunit;
using Moq;
using SchoolApp.UserRegistration.Models;
using SchoolApp.Repositories;
using SchoolApp.Models;

namespace TDDProjectDreamTeam.UserRegistration
{
    public class UserRegistrationServiceTests
    {
        private readonly UserRegistrationService _userRegistrationService;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public UserRegistrationServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userRegistrationService = new UserRegistrationService(_userRepositoryMock.Object);
        }

        [Fact]
        public void RegisterUser_Should_Succeed_With_Valid_Inputs()
        {
            // Arrange
            var name = "John Doe";
            var email = "john.doe@example.com";
            var password = "StrongPassword123!";
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).Throws(new KeyNotFoundException());

            // Act
            var result = _userRegistrationService.RegisterUser(name, email, password);

            // Assert
            Assert.True(result);
            _userRepositoryMock.Verify(repo => repo.AddUser(It.IsAny<User>()), Times.Once);
        }

        [Theory]
        [InlineData("", "john.doe@example.com", "StrongPassword123!")]
        [InlineData("John Doe", "", "StrongPassword123!")]
        [InlineData("John Doe", "john.doe@example.com", "")]
        public void RegisterUser_Should_Fail_With_Empty_Inputs(string name, string email, string password)
        {
            // Act
            var result = _userRegistrationService.RegisterUser(name, email, password);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("user@.com")]
        [InlineData("user@example")]
        [InlineData("userexample.com")]
        public void RegisterUser_Should_Fail_With_Invalid_Email_Format(string email)
        {
            // Arrange
            var name = "John Doe";
            var password = "StrongPassword123!";

            // Act
            var result = _userRegistrationService.RegisterUser(name, email, password);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("weak")]
        [InlineData("123456")]
        [InlineData("password")]
        public void RegisterUser_Should_Fail_With_Weak_Password(string password)
        {
            // Arrange
            var name = "John Doe";
            var email = "john.doe@example.com";

            // Act
            var result = _userRegistrationService.RegisterUser(name, email, password);

            // Assert
            Assert.False(result);
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
        public void ValidateName_Should_Fail_With_Special_Characters()
        {
            // Arrange
            var name = "Invalid@Name!";

            // Act
            var result = _userRegistrationService.ValidateName(name);

            // Assert
            Assert.False(result);
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
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).Returns(new User("test", email, null));

            // Act
            var result = _userRegistrationService.IsEmailAvailable(email);

            // Assert
            Assert.False(result);
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
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(emailUpper)).Throws(new KeyNotFoundException());
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(emailLower)).Throws(new KeyNotFoundException());

            // Act
            var isAvailableUpper = _userRegistrationService.IsEmailAvailable(emailUpper);
            var isAvailableLower = _userRegistrationService.IsEmailAvailable(emailLower);

            // Assert
            Assert.True(isAvailableUpper);
            Assert.True(isAvailableLower);
        }

        [Fact]
        public void RegisterUser_Should_Fail_With_Inputs_Exceeding_Max_Length()
        {
            // Arrange
            var name = new string('A', 256);
            var email = new string('B', 256) + "@example.com";
            var password = new string('C', 256);

            // Act
            var result = _userRegistrationService.RegisterUser(name, email, password);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("lowercase123!")]
        [InlineData("UPPERCASE123!")]
        [InlineData("NoDigits!")]
        [InlineData("NoSpecial123")]
        public void IsValidPassword_Should_Fail_With_Missing_Character_Types(string password)
        {
            // Act
            var result = _userRegistrationService.IsValidPassword(password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsEmailAvailable_Should_Return_True_For_Nonexistent_Email()
        {
            // Arrange
            var email = "nonexistent@example.com";
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).Throws<KeyNotFoundException>();

            // Act
            var result = _userRegistrationService.IsEmailAvailable(email);

            // Assert
            Assert.True(result);
        }
    }
}
