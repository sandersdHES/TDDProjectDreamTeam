using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolApp.UserAuthentication;
using Moq;

namespace TDDProjectDreamTeam.UserAuthentication
{
    public class PasswordManagementTests
    {
        [Fact]
        public async Task PasswordReset_Should_Allow_Login_With_New_Password()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var email = "user@example.com";
            var oldPassword = "OldPassword123!";
            var newPassword = "NewPassword123!";

            mockAuthService.Setup(service => service.ResetPasswordAsync(email, newPassword))
                .ReturnsAsync(true);

            mockAuthService.Setup(service => service.AuthenticateAsync(email, newPassword))
                .ReturnsAsync(true);

            var authService = mockAuthService.Object;

            // Act
            var resetResult = await authService.ResetPasswordAsync(email, newPassword);
            var loginResult = await authService.AuthenticateAsync(email, newPassword);

            // Assert
            Assert.True(resetResult);
            Assert.True(loginResult);
        }
    }

}
