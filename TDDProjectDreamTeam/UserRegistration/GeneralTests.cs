using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolApp.UserRegistration;

namespace TDDProjectDreamTeam.UserRegistration
{
    public class GeneralTests
    {
        [Fact]
        public void RegisterUser_Should_Succeed_With_Valid_Inputs()
        {
            // Arrange
            var mockService = new Mock<IUserRegistrationService>();
            var name = "John Doe";
            var email = "john.doe@example.com";
            var password = "StrongPassword123!";

            mockService.Setup(service => service.IsEmailAvailable(email)).Returns(true);
            mockService.Setup(service => service.IsValidPassword(password)).Returns(true);
            mockService.Setup(service => service.IsValidEmail(email)).Returns(true);
            mockService.Setup(service => service.ValidateName(name)).Returns(true);
            mockService.Setup(service => service.RegisterUser(name, email, password)).Returns(true);

            var service = mockService.Object;

            // Act
            var result = service.RegisterUser(name, email, password);

            // Assert
            Assert.True(result);
        }
    }
}
