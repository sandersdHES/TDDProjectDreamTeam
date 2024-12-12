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
    public class EmailValidationTests
    {
        // EmailValidationTests.cs
        [Fact]
        public void RegisterUser_Should_Fail_If_Email_Already_Exists()
        {
            // Arrange
            var service = new UserRegistrationService();
            var email = "existing.email@example.com";

            // Act
            var result = service.IsEmailAvailable(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Succeed_If_Email_Is_Available()
        {
            // Arrange
            var service = new UserRegistrationService();
            var email = "new.user@example.com";

            // Act
            var result = service.IsEmailAvailable(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void RegisterUser_Should_Fail_With_Invalid_Email_Format()
        {
            // Arrange
            var service = new UserRegistrationService();
            var email = "invalid-email";

            // Act
            var result = service.IsValidEmail(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Fail_If_Email_Is_Blank()
        {
            // Arrange
            var service = new UserRegistrationService();
            var email = "";

            // Act
            var result = service.IsValidEmail(email);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public void RegisterUser_Should_Handle_Email_Case_Insensitivity()
        {
            // Arrange
            var service = new UserRegistrationService();

            // Act
            var emailUpper = "USER@EXAMPLE.COM";
            var emailLower = "user@example.com";

            var isAvailableUpper = service.IsEmailAvailable(emailUpper); // Majuscules
            var isAvailableLower = service.IsEmailAvailable(emailLower); // Minuscules

            // Assert
            Assert.True(isAvailableUpper); // Devrait être True car déjà enregistré
            Assert.True(isAvailableLower); // Devrait être True car déjà enregistré
        }

        [Fact]
        public void RegisterUser_Should_Add_Email_To_RegisteredEmails()
        {
            // Arrange
            var service = new UserRegistrationService();
            var name = "John Doe";
            var email = "new.user@example.com";
            var password = "StrongPassword123!";

            // Act
            var result = service.RegisterUser(name, email, password);

            // Assert
            Assert.True(result); // L'enregistrement doit réussir
            Assert.False(service.IsEmailAvailable(email)); // L'e-mail doit être enregistré
        }
        [Fact]
        public void IsEmailAvailable_Should_Succeed_If_Email_With_Different_Case_Is_New()
        {
            var service = new UserRegistrationService();
            var email = "NEW.USER@example.com"; // Nouvelle casse

            var result = service.IsEmailAvailable(email);

            Assert.True(result); // L'e-mail doit être disponible
        }


    }

}
