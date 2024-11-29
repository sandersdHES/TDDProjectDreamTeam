using Moq;
using SchoolApp.ProfileManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolApp.ProfileManagement.Models;

namespace TDDProjectDreamTeam.ProfileManagement
{
    public class PasswordManagementTests
    {
        [Fact]
        public void UpdatePassword_WithCorrectCurrentPassword_ShouldReturnTrue()
        {
            // Arrange
            var mockService = new Mock<IUserProfileService>();
            mockService.Setup(service => service.UpdatePassword("user123", "currentPassword", "newStrongPassword"))
                       .Returns(true);

            // Act
            var result = mockService.Object.UpdatePassword("user123", "currentPassword", "newStrongPassword");

            // Assert
            Assert.True(result, "Password updates with the correct current password should succeed.");
        }

        [Fact]
        public void UpdatePassword_WithIncorrectCurrentPassword_ShouldReturnFalse()
        {
            // Arrange
            var mockService = new Mock<IUserProfileService>();
            mockService.Setup(service => service.UpdatePassword("user123", "wrongPassword", "newStrongPassword"))
                       .Returns(false);

            // Act
            var result = mockService.Object.UpdatePassword("user123", "wrongPassword", "newStrongPassword");

            // Assert
            Assert.False(result, "Password updates with an incorrect current password should fail.");
        }

        [Fact]
        public void UpdatePassword_WithWeakNewPassword_ShouldReturnFalse()
        {
            // Arrange
            var mockService = new Mock<IUserProfileService>();
            var weakPassword = "123";
            mockService.Setup(service => service.UpdatePassword("user123", "currentPassword", weakPassword))
                       .Returns(false);

            // Act
            var result = mockService.Object.UpdatePassword("user123", "currentPassword", weakPassword);

            // Assert
            Assert.False(result, "Weak passwords should not pass validation.");
        }
    }
}
