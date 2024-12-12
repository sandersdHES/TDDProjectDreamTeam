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
    public class NameValidationTests
    {
        // NameValidationTests.cs
        [Fact]
        public void RegisterUser_Should_Fail_If_Name_Is_Missing()
        {
            // Arrange
            var service = new UserRegistrationService();
            var name = "";

            // Act
            var result = service.ValidateName(name);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Fail_With_Invalid_Characters_In_Name()
        {
            // Arrange
            var service = new UserRegistrationService();
            var name = "John#Doe";

            // Act
            var result = service.ValidateName(name);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Succeed_With_Valid_Name()
        {
            // Arrange
            var service = new UserRegistrationService();
            var name = "John Doe";

            // Act
            var result = service.ValidateName(name);

            // Assert
            Assert.True(result);
        }
        [Fact]
        public void ValidateName_Should_Fail_With_Special_Characters()
        {
            // Arrange
            var service = new UserRegistrationService();
            var invalidName = "John#Doe";

            // Act
            var result = service.ValidateName(invalidName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateName_Should_Fail_If_Name_Is_Too_Short()
        {
            // Arrange
            var service = new UserRegistrationService();
            var shortName = "A";

            // Act
            var result = service.ValidateName(shortName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateName_Should_Pass_With_Valid_Name()
        {
            // Arrange
            var service = new UserRegistrationService();
            var validName = "John Doe";

            // Act
            var result = service.ValidateName(validName);

            // Assert
            Assert.True(result);
        }

    }

}
