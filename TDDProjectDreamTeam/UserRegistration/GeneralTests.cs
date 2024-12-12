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
    public class GeneralTests
    {
        // GeneralTests.cs
        [Fact]
        public void RegisterUser_Should_Succeed_With_Valid_Inputs()
        {
            // Arrange
            var service = new UserRegistrationService();
            var name = "John Doe";
            var email = "john.doe@example.com";
            var password = "StrongPassword123!";

            // Act
            var result = service.RegisterUser(name, email, password);

            // Assert
            Assert.True(result);
        }
        [Fact]
        public void RegisterUser_Should_Succeed_With_Max_Length_Inputs()
        {
            var service = new UserRegistrationService();
            var name = new string('a', 255);
            var email = $"{new string('a', 243)}@example.com"; // Max 255 caractères
            var password = "StrongPassword123!";

            var result = service.RegisterUser(name, email, password);

            Assert.True(result);
        }
        [Fact]
        public void RegisterUser_Should_Fail_With_Empty_Inputs()
        {
            var service = new UserRegistrationService();

            var result = service.RegisterUser("", "", "");

            Assert.False(result);
        }

    }
}
