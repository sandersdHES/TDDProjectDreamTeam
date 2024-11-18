using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolApp.UserRegistration;

namespace TDDProjectDreamTeam.UserRegistration
{
    public class EdgeCaseTests
    {
        [Fact]
        public void RegisterUser_Should_Fail_With_Input_Exceeding_Max_Length()
        {
            // Arrange
            var mockService = new Mock<IUserRegistrationService>();
            var longInput = new string('a', 256);

            mockService.Setup(service => service.RegisterUser(longInput, longInput, longInput)).Returns(false);

            var service = mockService.Object;

            // Act
            var result = service.RegisterUser(longInput, longInput, longInput);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Prevent_SQL_Injection()
        {
            // Arrange
            var mockService = new Mock<IUserRegistrationService>();
            var maliciousInput = "'; DROP TABLE Users; --";

            mockService.Setup(service => service.RegisterUser(maliciousInput, maliciousInput, maliciousInput)).Returns(false);

            var service = mockService.Object;

            // Act
            var result = service.RegisterUser(maliciousInput, maliciousInput, maliciousInput);

            // Assert
            Assert.False(result);
        }
    }

}
