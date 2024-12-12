using SchoolApp.UserRegistration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDProjectDreamTeam.UserRegistration.Tests
{
    public class UserRegistrationServiceTests
    {
        [Fact]
        public void RegisterUser_Should_Succeed_With_Valid_Inputs()
        {
            var service = new UserRegistrationService();
            var name = "Valid Name";
            var email = "valid.email@example.com";
            var password = "ValidPassword123!";

            var result = service.RegisterUser(name, email, password);

            Assert.True(result);
        }


        [Fact]
        public void RegisterUser_Should_Fail_If_Name_Too_Long()
        {
            var service = new UserRegistrationService();
            var longName = new string('a', 256);
            var email = "valid.email@example.com";
            var password = "ValidPassword123!";

            var result = service.RegisterUser(longName, email, password);

            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Fail_With_Invalid_Email_Format()
        {
            var service = new UserRegistrationService();
            var name = "Valid Name";
            var email = "invalid-email";
            var password = "ValidPassword123!";

            var result = service.RegisterUser(name, email, password);

            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Fail_With_Weak_Password()
        {
            var service = new UserRegistrationService();
            var name = "Valid Name";
            var email = "valid.email@example.com";
            var password = "weak";

            var result = service.RegisterUser(name, email, password);

            Assert.False(result);
        }

        [Fact]
        public void RegisterUser_Should_Add_Email_To_RegisteredEmails()
        {
            var service = new UserRegistrationService();
            var name = "John Doe";
            var email = "new.user@example.com";
            var password = "StrongPassword123!";

            var result = service.RegisterUser(name, email, password);

            Assert.True(result);
            Assert.False(service.IsEmailAvailable(email));
        }

        [Fact]
        public void ValidateName_Should_Fail_If_Name_Is_Too_Short()
        {
            var service = new UserRegistrationService();
            var shortName = "A";

            var result = service.ValidateName(shortName);

            Assert.False(result);
        }

        [Fact]
        public void ValidateName_Should_Succeed_With_Valid_Name()
        {
            var service = new UserRegistrationService();
            var validName = "John Doe";

            var result = service.ValidateName(validName);

            Assert.True(result);
        }

        [Fact]
        public void IsValidEmail_Should_Succeed_With_Valid_Email()
        {
            var service = new UserRegistrationService();
            var email = "user@example.com";

            var result = service.IsValidEmail(email);

            Assert.True(result);
        }

        [Fact]
        public void IsEmailAvailable_Should_Fail_If_Email_Exists()
        {
            var service = new UserRegistrationService();
            var email = "existing.email@example.com";

            var result = service.IsEmailAvailable(email);

            Assert.False(result);
        }

        [Fact]
        public void IsValidPassword_Should_Fail_With_No_Special_Character()
        {
            var service = new UserRegistrationService();
            var password = "Password123";

            var result = service.IsValidPassword(password);

            Assert.False(result);
        }

        [Fact]
        public void IsValidPassword_Should_Succeed_With_Strong_Password()
        {
            var service = new UserRegistrationService();
            var password = "Strong@Password123";

            var result = service.IsValidPassword(password);

            Assert.True(result);
        }

        [Fact]
        public void RegisterUser_Should_Succeed_With_Max_Length_Inputs()
        {
            var service = new UserRegistrationService();
            var name = new string('a', 255);
            var email = $"{new string('a', 243)}@example.com";
            var password = "StrongPassword123!";

            var result = service.RegisterUser(name, email, password);

            Assert.True(result);
        }

        [Fact]
        public void RegisterUser_Should_Fail_With_Empty_Inputs()
        {
            var service = new UserRegistrationService();

            var result = service.RegisterUser("", "", "");

            Assert.False(result);
        }
    }
}
