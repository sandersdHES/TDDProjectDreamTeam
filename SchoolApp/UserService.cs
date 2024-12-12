using SchoolApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolApp
{
    public class UserService : IUserService
    {
        private readonly List<User> users;
        private readonly Action<string> logger;

        public UserService(Action<string> logger = null)
        {
            users = new List<User>();
            this.logger = logger ?? Console.WriteLine;
        }

        public User RegisterUser(User user, string password)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            if (users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("A user with this email already exists.");

            user.PasswordHash = HashPassword(password);
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
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            logger?.Invoke($"User '{user.Name}' authenticated.");
            return user;
        }

        public User GetUserById(int userId)
        {
            var user = users.FirstOrDefault(u => u.Id.Equals(userId.ToString(), StringComparison.OrdinalIgnoreCase));
            if (user == null)
                throw new KeyNotFoundException($"User with ID '{userId}' not found.");

            return user;
        }

        private string HashPassword(string password)
        {
            // Implement a hashing algorithm here
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            // Implement password verification here
            return passwordHash == Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
