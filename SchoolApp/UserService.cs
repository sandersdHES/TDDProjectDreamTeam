using SchoolApp.Models;
using SchoolApp.Repositories;
using SchoolApp.UserAuthentication;
using SchoolApp.UserRegistration;
using System;

namespace SchoolApp
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashingService _hashingService;
        private readonly IUserRegistrationService _userRegistrationService;

        public UserService(IUserRepository userRepository, IHashingService hashingService, IUserRegistrationService userRegistrationService, Action<string> logger = null)
        {
            _userRepository = userRepository;
            _hashingService = hashingService;
            _userRegistrationService = userRegistrationService;
        }

        public User RegisterUser(User user, string password)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!_userRegistrationService.RegisterUser(user.Name, user.Email, password))
                throw new ArgumentException("Invalid user registration details.");

            user.PasswordHash = _hashingService.HashPassword(password);
            _userRepository.AddUser(user);

            return user;
        }

        public User AuthenticateUser(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            var user = _userRepository.GetUserByEmail(email);
            if (user == null || !_hashingService.VerifyPassword(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");
            return user;
        }

        public User GetUserById(string userId)
        {
            var user = _userRepository.GetUser(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID '{userId}' not found.");

            return user;
        }
    }
}
