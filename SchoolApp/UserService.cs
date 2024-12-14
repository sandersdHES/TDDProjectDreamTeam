using SchoolApp.Models;
using SchoolApp.UserAuthentication;
using SchoolApp.UserAuthentication.Models;
using SchoolApp.UserRegistration;
using SchoolApp.UserRegistration.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolApp
{
    public class UserService : IUserService
    {
        private readonly List<User> users;
        private readonly Action<string> logger;
        private readonly IHashingService hashingService;
        private readonly IUserRegistrationService userRegistrationService;

        public UserService(List<User> users,IHashingService hashingService, IUserRegistrationService userRegistrationService, Action<string> logger = null)
        {
            this.users = users;
            this.logger = logger ?? Console.WriteLine;
            this.hashingService = hashingService;
            this.userRegistrationService = userRegistrationService;
        }

        public User RegisterUser(User user, string password)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!userRegistrationService.RegisterUser(user.Name, user.Email, password))
                throw new ArgumentException("Invalid user registration details.");

            user.PasswordHash = hashingService.HashPassword(password);
            users.Add(user);
            logger?.Invoke($"User '{user.Name}' registered with email '{user.Email}'.");

            return user;
        }

        public User AuthenticateUser(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            var user = users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user == null || !hashingService.VerifyPassword(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            logger?.Invoke($"User '{user.Name}' authenticated.");
            return user;
        }

        public User GetUserById(string userId)
        {
            var user = users.FirstOrDefault(u => u.Id.Equals(userId, StringComparison.OrdinalIgnoreCase));
            if (user == null)
                throw new KeyNotFoundException($"User with ID '{userId}' not found.");

            return user;
        }
    }
}
