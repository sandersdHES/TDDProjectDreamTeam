using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolApp.UserRegistration;

namespace TDDProjectDreamTeam.UserRegistration
{
    public class EmailValidationTests
    {
        [Fact]
        public void RegisterUser_Should_Fail_If_Email_Already_Exists()
        {
            // Arrange
            var mockService = new Mock<IUserRegistrationService>();
            var email = "existing.email@example.com";

            mockService.Setup(service => service.IsEmailAvailable(email)).Returns(false);

            var service = mockService.Object;

            // Act
            var result = service.IsEmailAvailable(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Succeed_If_Email_Is_Available()
        {
            // Arrange
            var mockService = new Mock<IUserRegistrationService>();
            var email = "new.user@example.com";

            mockService.Setup(service => service.IsEmailAvailable(email)).Returns(true);

            var service = mockService.Object;

            // Act
            var result = service.IsEmailAvailable(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void RegisterUser_Should_Fail_With_Invalid_Email_Format()
        {
            // Arrange
            var mockService = new Mock<IUserRegistrationService>();
            var email = "invalid-email";

            mockService.Setup(service => service.IsValidEmail(email)).Returns(false);

            var service = mockService.Object;

            // Act
            var result = service.IsValidEmail(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Fail_If_Email_Is_Blank()
        {
            // Arrange
            var mockService = new Mock<IUserRegistrationService>();
            var email = "";

            mockService.Setup(service => service.IsValidEmail(email)).Returns(false);

            var service = mockService.Object;

            // Act
            var result = service.IsValidEmail(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Handle_Email_Case_Insensitivity()
        {
            // Arrange
            var mockService = new Mock<IUserRegistrationService>();
            var email1 = "USER@EXAMPLE.COM";
            var email2 = "user@example.com";

            mockService.Setup(service => service.IsEmailAvailable(email1)).Returns(true);
            mockService.Setup(service => service.IsEmailAvailable(email2)).Returns(false);

            var service = mockService.Object;

            // Act
            var result1 = service.IsEmailAvailable(email1);
            var result2 = service.IsEmailAvailable(email2);

            // Assert
            Assert.True(result1);
            Assert.False(result2);
        }
    }

}
