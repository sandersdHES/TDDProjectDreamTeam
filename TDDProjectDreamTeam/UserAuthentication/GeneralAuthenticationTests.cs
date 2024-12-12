using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolApp.UserAuthentication;
using Moq;

namespace TDDProjectDreamTeam.UserAuthentication
{
    public class GeneralAuthenticationTests
    {
        [Fact]
        public async Task SuccessfulAuthentication_Should_Return_True_When_Credentials_Are_Valid()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var validEmail = "user@example.com";
            var validPassword = "Password123!";

            mockAuthService.Setup(service => service.AuthenticateAsync(validEmail, validPassword))
                .ReturnsAsync(true);

            var authService = mockAuthService.Object;

            // Act
            var result = await authService.AuthenticateAsync(validEmail, validPassword);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task InvalidCredentials_Should_Return_False_When_Email_Or_Password_Is_Invalid()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var invalidEmail = "invalid@example.com";
            var invalidPassword = "WrongPassword!";

            mockAuthService.Setup(service => service.AuthenticateAsync(invalidEmail, invalidPassword))
                .ReturnsAsync(false);

            var authService = mockAuthService.Object;

            // Act
            var result = await authService.AuthenticateAsync(invalidEmail, invalidPassword);

            // Assert
            Assert.False(result);
        }

    }

}
