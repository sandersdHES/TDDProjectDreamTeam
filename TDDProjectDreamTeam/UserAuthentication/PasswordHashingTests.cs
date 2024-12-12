using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolApp.UserAuthentication;
using Moq;

namespace TDDProjectDreamTeam.UserAuthentication
{
    public class PasswordHashingTests
    {
        [Fact]
        public void Password_Should_Be_Hashed_Before_Storage()
        {
            // Arrange
            var password = "Password123!";
            var mockHashingService = new Mock<IHashingService>();

            mockHashingService.Setup(service => service.HashPassword(password))
                .Returns("hashedpassword123");

            var hashingService = mockHashingService.Object;

            // Act
            var hashedPassword = hashingService.HashPassword(password);

            // Assert
            Assert.NotEqual(password, hashedPassword);
        }

        [Fact]
        public void Password_Hash_Should_Be_Unique_For_Different_Passwords()
        {
            // Arrange
            var password1 = "Password123!";
            var password2 = "AnotherPassword123!";
            var mockHashingService = new Mock<IHashingService>();

            mockHashingService.Setup(service => service.HashPassword(password1))
                .Returns("hashedpassword1");

            mockHashingService.Setup(service => service.HashPassword(password2))
                .Returns("hashedpassword2");

            var hashingService = mockHashingService.Object;

            // Act
            var hash1 = hashingService.HashPassword(password1);
            var hash2 = hashingService.HashPassword(password2);

            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void Password_Should_Be_Verified_Against_Stored_Hash()
        {
            // Arrange
            var password = "Password123!";
            var hash = "hashedpassword123";
            var mockHashingService = new Mock<IHashingService>();

            mockHashingService.Setup(service => service.VerifyPassword(password, hash))
                .Returns(true);

            var hashingService = mockHashingService.Object;

            // Act
            var isVerified = hashingService.VerifyPassword(password, hash);

            // Assert
            Assert.True(isVerified);
        }
    }

}
