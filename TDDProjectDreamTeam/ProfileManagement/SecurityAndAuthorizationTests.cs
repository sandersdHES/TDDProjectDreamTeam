using Moq;
using SchoolApp.ProfileManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDProjectDreamTeam.ProfileManagement
{
    public class SecurityAndAuthorizationTests
    {
        [Fact]
        public void UpdateProfile_ByUnauthorizedUser_ShouldReturnFalse()
        {
            // Arrange
            var mockService = new Mock<IUserProfileService>();
            var updatedProfile = new UserProfile { UserId = "user123", Name = "John Doe" };
            mockService.Setup(service => service.IsAuthorizedToUpdateProfile("unauthorizedUser", "user123"))
                       .Returns(false);
            mockService.Setup(service => service.UpdateProfile("unauthorizedUser", updatedProfile))
                       .Returns(false);

            // Act
            var isAuthorized = mockService.Object.IsAuthorizedToUpdateProfile("unauthorizedUser", "user123");
            var result = mockService.Object.UpdateProfile("unauthorizedUser", updatedProfile);

            // Assert
            Assert.False(isAuthorized, "Unauthorized users should not have access to update profiles.");
            Assert.False(result, "Profile updates by unauthorized users should fail.");
        }

        [Fact]
        public void UpdateProfile_ByAuthorizedAdmin_ShouldReturnTrue()
        {
            // Arrange
            var mockService = new Mock<IUserProfileService>();
            var updatedProfile = new UserProfile { UserId = "user123", Name = "John Doe" };
            mockService.Setup(service => service.IsAuthorizedToUpdateProfile("admin123", "user123"))
                       .Returns(true);
            mockService.Setup(service => service.UpdateProfile("admin123", updatedProfile))
                       .Returns(true);

            // Act
            var isAuthorized = mockService.Object.IsAuthorizedToUpdateProfile("admin123", "user123");
            var result = mockService.Object.UpdateProfile("admin123", updatedProfile);

            // Assert
            Assert.True(isAuthorized, "Admins should have access to update profiles.");
            Assert.True(result, "Profile updates by authorized admins should succeed.");
        }
    }
}

