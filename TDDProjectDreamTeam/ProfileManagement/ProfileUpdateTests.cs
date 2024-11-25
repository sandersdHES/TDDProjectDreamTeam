using Moq;
using SchoolApp.ProfileManagement;

namespace TDDProjectDreamTeam.ProfileManagement
{
    public class ProfileUpdateTests
    {
        [Fact]
        public void UpdateProfile_WithValidData_ShouldReturnTrue()
        {
            // Arrange
            var mockService = new Mock<IUserProfileService>();
            var updatedProfile = new UserProfile { UserId = "user123", Name = "Jane Doe", Email = "jane.doe@example.com" };
            mockService.Setup(service => service.UpdateProfile("user123", updatedProfile)).Returns(true);

            // Act
            var result = mockService.Object.UpdateProfile("user123", updatedProfile);

            // Assert
            Assert.True(result, "Profile updates with valid data should succeed.");
        }

        [Fact]
        public void UpdateProfile_WithInvalidEmail_ShouldReturnFalse()
        {
            // Arrange
            var mockService = new Mock<IUserProfileService>();
            var updatedProfile = new UserProfile { UserId = "user123", Email = "invalid-email" };
            mockService.Setup(service => service.IsEmailValid("invalid-email")).Returns(false);
            mockService.Setup(service => service.UpdateProfile("user123", updatedProfile)).Returns(false);

            // Act
            var isValid = mockService.Object.IsEmailValid("invalid-email");
            var result = mockService.Object.UpdateProfile("user123", updatedProfile);

            // Assert
            Assert.False(isValid, "Invalid emails should not pass validation.");
            Assert.False(result, "Profile updates with invalid email should fail.");
        }

        [Fact]
        public void UpdateProfile_WithEmptyMandatoryField_ShouldReturnFalse()
        {
            // Arrange
            var mockService = new Mock<IUserProfileService>();
            var updatedProfile = new UserProfile { UserId = "user123", Name = "", Email = "jane.doe@example.com" };
            mockService.Setup(service => service.IsFieldMandatory("Name")).Returns(true);
            mockService.Setup(service => service.UpdateProfile("user123", updatedProfile)).Returns(false);

            // Act
            var isMandatory = mockService.Object.IsFieldMandatory("Name");
            var result = mockService.Object.UpdateProfile("user123", updatedProfile);

            // Assert
            Assert.True(isMandatory, "Name should be a mandatory field.");
            Assert.False(result, "Profile updates with empty mandatory fields should fail.");
        }

        [Fact]
        public void UpdateProfile_SimultaneousUpdates_ShouldHandleGracefully()
        {
            // Arrange
            var mockService = new Mock<IUserProfileService>();
            var updatedProfile1 = new UserProfile { UserId = "user123", Name = "Jane Doe" };
            var updatedProfile2 = new UserProfile { UserId = "user123", Name = "John Smith" };
            mockService.SetupSequence(service => service.UpdateProfile("user123", It.IsAny<UserProfile>()))
                       .Returns(true)
                       .Returns(true);

            // Act
            var result1 = mockService.Object.UpdateProfile("user123", updatedProfile1);
            var result2 = mockService.Object.UpdateProfile("user123", updatedProfile2);

            // Assert
            Assert.True(result1, "First simultaneous update should succeed.");
            Assert.True(result2, "Second simultaneous update should succeed.");
        }

        [Fact]
        public void UpdateProfile_PartialUpdate_ShouldRetainUnchangedFields()
        {
            // Arrange
            var mockService = new Mock<IUserProfileService>();
            var existingProfile = new UserProfile { UserId = "user123", Name = "Jane Doe", Email = "jane.doe@example.com" };
            var partialUpdate = new UserProfile { UserId = "user123", Name = "Jane Updated" }; // Email not updated

            mockService.Setup(service => service.GetProfile("user123")).Returns(existingProfile);
            mockService.Setup(service => service.UpdateProfile("user123", It.IsAny<UserProfile>())).Returns(true);

            // Act
            var updatedProfile = mockService.Object.GetProfile("user123");
            updatedProfile.Name = partialUpdate.Name; // Apply partial update
            var result = mockService.Object.UpdateProfile("user123", updatedProfile);

            // Assert
            Assert.True(result, "Partial updates should succeed.");
            Assert.Equal("Jane Updated", updatedProfile.Name);
            Assert.Equal("jane.doe@example.com", updatedProfile.Email);
        }

        [Fact]
        public void UpdateProfile_WithDuplicateEmail_ShouldReturnFalse()
        {
            // Arrange
            var mockService = new Mock<IUserProfileService>();
            mockService.Setup(service => service.IsEmailValid(It.IsAny<string>())).Returns(true);
            mockService.Setup(service => service.UpdateProfile("user123", It.Is<UserProfile>(p => p.Email == "duplicate@example.com")))
                       .Returns(false);

            // Act
            var result = mockService.Object.UpdateProfile("user123", new UserProfile { UserId = "user123", Email = "duplicate@example.com" });

            // Assert
            Assert.False(result, "Updating profile with a duplicate email should fail.");
        }


        [Fact]
        public void UpdateProfile_ImmutableField_ShouldReturnFalse()
        {
            // Arrange
            var mockService = new Mock<IUserProfileService>();
            var updatedProfile = new UserProfile { UserId = "newUserId" }; // Attempting to change User ID
            mockService.Setup(service => service.UpdateProfile("user123", It.IsAny<UserProfile>())).Returns(false);

            // Act
            var result = mockService.Object.UpdateProfile("user123", updatedProfile);

            // Assert
            Assert.False(result, "Updates to immutable fields should be rejected.");
        }

    }
}
