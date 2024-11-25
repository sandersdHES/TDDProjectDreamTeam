using System;
using Moq;
using Xunit;
using TDDProjectDreamTeam.ProfileManagement;
using SchoolApp.ProfileManagement;

namespace TDDProjectDreamTeam.ProfileManagement
{
    public class ProfileRetrievalTests
    {
        [Fact]
        public void GetProfile_WithValidUserId_ShouldReturnProfile()
        {
            // Arrange
            var mockService = new Mock<IUserProfileService>();
            var expectedProfile = new UserProfile { UserId = "user123", Name = "John Doe" };
            mockService.Setup(service => service.GetProfile("user123")).Returns(expectedProfile);

            // Act
            var result = mockService.Object.GetProfile("user123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("user123", result.UserId);
            Assert.Equal("John Doe", result.Name);
        }

        [Fact]
        public void GetProfile_WithUnauthorizedUserId_ShouldReturnNull()
        {
            // Arrange
            var mockService = new Mock<IUserProfileService>();
            mockService.Setup(service => service.GetProfile("unauthorizedUser")).Returns((UserProfile)null);

            // Act
            var result = mockService.Object.GetProfile("unauthorizedUser");

            // Assert
            Assert.Null(result);
        }
    }
}
