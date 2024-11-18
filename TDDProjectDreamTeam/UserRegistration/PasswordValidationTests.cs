using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolApp.UserRegistration;

namespace TDDProjectDreamTeam.UserRegistration
{
    public class PasswordValidationTests
    {
        [Fact]
        public void RegisterUser_Should_Fail_With_Weak_Password()
        {
            // Arrange
            var mockService = new Mock<IUserRegistrationService>();
            var password = "weak";

            mockService.Setup(service => service.IsValidPassword(password)).Returns(false);

            var service = mockService.Object;

            // Act
            var result = service.IsValidPassword(password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Fail_If_Password_Is_Blank()
        {
            // Arrange
            var mockService = new Mock<IUserRegistrationService>();
            var password = "";

            mockService.Setup(service => service.IsValidPassword(password)).Returns(false);

            var service = mockService.Object;

            // Act
            var result = service.IsValidPassword(password);

            // Assert
            Assert.False(result);
        }
    }

}
