using System;
using System.Collections.Generic;
using System.Linq;
using SchoolApp.Models;
using SchoolApp.Repositories;
using SchoolApp.UserAuthentication;

namespace SchoolApp.ProfileManagement
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashingService _hashingService;

        public UserProfileService(IUserRepository userRepository, IHashingService hashingService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _hashingService = hashingService ?? throw new ArgumentNullException(nameof(hashingService));
        }

        public User GetProfile(string userId)
        {
            return _userRepository.GetUser(userId);
        }

        public bool UpdateProfile(string userId, User updatedUser)
        {
            var existingUser = _userRepository.GetUser(userId);
            if (existingUser == null)
                return false;

            if (!string.IsNullOrWhiteSpace(updatedUser.Email) && !IsEmailValid(updatedUser.Email))
                return false;

            if (!string.IsNullOrWhiteSpace(updatedUser.Email) && existingUser.Email != updatedUser.Email && _userRepository.GetUserByEmail(updatedUser.Email) != null)
                return false;

            if (string.IsNullOrWhiteSpace(updatedUser.Name) || updatedUser.Name.Length > 50)
                return false;

            existingUser.Name = updatedUser.Name ?? existingUser.Name;
            existingUser.Email = updatedUser.Email ?? existingUser.Email;
            existingUser.Role = updatedUser.Role ?? existingUser.Role;

            _userRepository.UpdateUser(existingUser);
            return true;
        }

        public bool UpdatePassword(string userId, string currentPassword, string newPassword)
        {
            var user = _userRepository.GetUser(userId);
            if (user == null || !_hashingService.VerifyPassword(currentPassword, user.PasswordHash))
                return false;

            if (!IsPasswordStrong(newPassword))
                return false;

            user.PasswordHash = _hashingService.HashPassword(newPassword);
            _userRepository.UpdateUser(user);
            return true;
        }

        public bool IsAuthorizedToUpdateProfile(string userId, string profileId)
        {
            var user = _userRepository.GetUser(userId);
            if (user == null)
                return false;

            return userId == profileId || (user.Role != null && user.Role.Name == "Admin" && user.Role.Permissions.Contains("ManageUsers"));
        }

        private bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", System.Text.RegularExpressions.RegexOptions.Compiled);
            return emailRegex.IsMatch(email);
        }

        private bool IsPasswordStrong(string password)
        {
            return password.Length >= 8 &&
                   System.Text.RegularExpressions.Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
        }
    }
}
