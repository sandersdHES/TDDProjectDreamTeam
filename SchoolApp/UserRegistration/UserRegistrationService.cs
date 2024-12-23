using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SchoolApp.Models;
using SchoolApp.Repositories;

namespace SchoolApp.UserRegistration.Models
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private const int MaxLength = 255;
        private readonly IUserRepository _userRepository;

        public UserRegistrationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool RegisterUser(string name, string email, string password)
        {
            // Validate input lengths
            if (!IsValidLength(name, email, password))
            {
                return false;
            }

            if (!ValidateName(name) || !IsValidEmail(email) || !IsValidPassword(password))
            {
                return false;
            }
            // Check email availability
            if (!IsEmailAvailable(email))
            {
                return false;
            }

            var user = new User(name, name, email, password, null );
            _userRepository.AddUser(user);

            return true;
        }

        private bool IsValidLength(string name, string email, string password)
        {
            return name.Length <= MaxLength && email.Length <= MaxLength && password.Length <= MaxLength;
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
            try
            {
                _userRepository.GetUserByEmail(email);
                return false;
            }
            catch (KeyNotFoundException)
            {
                return true;
            }
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
    }

}
