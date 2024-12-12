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
    public class PasswordValidationTests
    {
        // PasswordValidationTests.cs
        [Fact]
        public void RegisterUser_Should_Fail_With_Weak_Password()
        {
            // Arrange
            var service = new UserRegistrationService();
            var password = "weak";

            // Act
            var result = service.IsValidPassword(password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Fail_If_Password_Is_Blank()
        {
            // Arrange
            var service = new UserRegistrationService();
            var password = "";

            // Act
            var result = service.IsValidPassword(password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Succeed_With_Strong_Password()
        {
            // Arrange
            var service = new UserRegistrationService();
            var password = "StrongPassword123!";

            // Act
            var result = service.IsValidPassword(password);

            // Assert
            Assert.True(result);
        }
        [Fact]
        public void IsValidPassword_Should_Fail_With_No_Special_Character()
        {
            var service = new UserRegistrationService();
            var password = "Password123";

            var result = service.IsValidPassword(password);

            Assert.False(result);
        }

    }

}
