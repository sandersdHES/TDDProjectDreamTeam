using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolApp.UserRegistration.Models
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private const int MaxLength = 255;
        public readonly HashSet<string> _registeredEmails;

        public UserRegistrationService()
        {
            _registeredEmails = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "existing.email@example.com",
                "test.user@example.com"
            };
        }


        public bool RegisterUser(string name, string email, string password)
        {
            // Validate input lengths
            if (name.Length > MaxLength || email.Length > MaxLength || password.Length > MaxLength)
            {
                return false;
            }

            // Validate name
            if (!ValidateName(name))
            {
                return false;
            }

            // Validate email
            if (!IsValidEmail(email))
            {
                return false;
            }

            // Check email availability
            if (!IsEmailAvailable(email))
            {
                return false;
            }

            // Validate password
            if (!IsValidPassword(password))
            {
                return false;
            }

            // Save the user to the database (mocked)
            return SaveUserToDatabase(name, email, password);
        }

        public bool ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            if (name.Length < 2)
            {
                return false;
            }

            var nameRegex = new Regex(@"^[a-zA-Z\s\-']+$");
            return nameRegex.IsMatch(name);
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            email.ToLower();
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        public bool IsEmailAvailable(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            return !_registeredEmails.Contains(email);
        }

        public bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            return passwordRegex.IsMatch(password);
        }

        private bool SaveUserToDatabase(string name, string email, string password)
        {
            // Mock saving the user to a database
            _registeredEmails.Add(email);
            return true;
        }
    }

}
