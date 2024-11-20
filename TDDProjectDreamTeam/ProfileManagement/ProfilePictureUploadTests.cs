using Moq;
using SchoolApp.ProfileManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDProjectDreamTeam.ProfileManagement
{
    public class ProfilePictureUploadTests
    {
        [Fact]
        public void UploadProfilePicture_WithValidFile_ShouldReturnTrue()
        {
            // Arrange
            var mockService = new Mock<IUserProfileService>();
            var validPicture = new byte[] { 1, 2, 3 }; // Mock valid image byte array
            mockService.Setup(service => service.UploadProfilePicture("user123", validPicture))
                       .Returns(true);

            // Act
            var result = mockService.Object.UploadProfilePicture("user123", validPicture);

            // Assert
            Assert.True(result, "Valid profile pictures should be uploaded successfully.");
        }

        [Fact]
        public void UploadProfilePicture_WithInvalidFile_ShouldReturnFalse()
        {
            // Arrange
            var mockService = new Mock<IUserProfileService>();
            var invalidPicture = new byte[] { }; // Mock invalid image byte array
            mockService.Setup(service => service.UploadProfilePicture("user123", invalidPicture))
                       .Returns(false);

            // Act
            var result = mockService.Object.UploadProfilePicture("user123", invalidPicture);

            // Assert
            Assert.False(result, "Invalid profile pictures should not be accepted.");
        }

        [Fact]
        public void UploadProfilePicture_WithExceedingFileSize_ShouldReturnFalse()
        {
            // Arrange
            var mockService = new Mock<IUserProfileService>();
            var largePicture = new byte[10_000_001]; // Mock oversized image
            mockService.Setup(service => service.UploadProfilePicture("user123", largePicture))
                       .Returns(false);

            // Act
            var result = mockService.Object.UploadProfilePicture("user123", largePicture);

            // Assert
            Assert.False(result, "Profile pictures exceeding the allowed size should be rejected.");
        }
    }
}
