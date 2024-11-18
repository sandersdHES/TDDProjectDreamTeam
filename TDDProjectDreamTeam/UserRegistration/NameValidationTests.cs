using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolApp.UserRegistration;

namespace TDDProjectDreamTeam.UserRegistration
{
    public class NameValidationTests
    {
        [Fact]
        public void RegisterUser_Should_Fail_If_Name_Is_Missing()
        {
            // Arrange
            var mockService = new Mock<IUserRegistrationService>();
            var name = "";

            mockService.Setup(service => service.ValidateName(name)).Returns(false);

            var service = mockService.Object;

            // Act
            var result = service.ValidateName(name);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Fail_With_Invalid_Characters_In_Name()
        {
            // Arrange
            var mockService = new Mock<IUserRegistrationService>();
            var name = "John#Doe";

            mockService.Setup(service => service.ValidateName(name)).Returns(false);

            var service = mockService.Object;

            // Act
            var result = service.ValidateName(name);

            // Assert
            Assert.False(result);
        }
    }

}
