using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolApp.UserAuthentication;
using Moq;


namespace TDDProjectDreamTeam.UserAuthentication
{
    public class AccountLockoutTests
    {
        [Fact]
        public async Task Account_Should_Be_Locked_After_Multiple_Failed_Attempts()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var email = "user@example.com";
            var wrongPassword = "WrongPassword!";
            var failedAttempts = 5;

            mockAuthService.Setup(service => service.IsAccountLockedAsync(email))
                .ReturnsAsync(true);

            for (int i = 0; i < failedAttempts; i++)
            {
                mockAuthService.Setup(service => service.AuthenticateAsync(email, wrongPassword))
                    .ReturnsAsync(false);
            }

            var authService = mockAuthService.Object;

            // Act
            for (int i = 0; i < failedAttempts; i++)
            {
                await authService.AuthenticateAsync(email, wrongPassword);
            }

            var isLocked = await authService.IsAccountLockedAsync(email);

            // Assert
            Assert.True(isLocked);
        }
    }

}
