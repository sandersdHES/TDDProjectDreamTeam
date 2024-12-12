using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolApp.UserRegistration;
using SchoolApp.UserRegistration.Models;

namespace TDDProjectDreamTeam.UserRegistration
{
    public class EdgeCaseTests
    {
        // EdgeCaseTests.cs
        [Fact]
        public void RegisterUser_Should_Fail_With_Input_Exceeding_Max_Length()
        {
            // Arrange
            var service = new UserRegistrationService();
            var longInput = new string('a', 256);

            // Act
            var result = service.RegisterUser(longInput, longInput, longInput);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Prevent_SQL_Injection()
        {
            // Arrange
            var service = new UserRegistrationService();
            var maliciousInput = "'; DROP TABLE Users; --";

            // Act
            var result = service.RegisterUser(maliciousInput, maliciousInput, maliciousInput);

            // Assert
            Assert.False(result);
        }
    }

}
