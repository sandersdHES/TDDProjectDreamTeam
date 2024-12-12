using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BCrypt.Net;
using SchoolApp.ProfileManagement.Models;


namespace SchoolApp.ProfileManagement
{
    public class UserProfileService : IUserProfileService
    {
        private readonly Dictionary<string, UserProfile> _userProfiles = new();
        private readonly HashSet<string> _usedEmails = new(StringComparer.OrdinalIgnoreCase);

        public UserProfile GetProfile(string userId)
        {
            if (_userProfiles.TryGetValue(userId, out var profile))
            {
                return profile;
            }

            return null; // User not found
        }

        public bool UpdateProfile(string userId, UserProfile updatedProfile)
        {
            if (!_userProfiles.ContainsKey(userId))
                return false;

            if (!IsEmailValid(updatedProfile.Email))
                return false;

            if (_usedEmails.Contains(updatedProfile.Email) && _userProfiles[userId].Email != updatedProfile.Email)
                return false;

            if (!IsFieldMandatory(updatedProfile.Name))
                return false;

            if (!IsFieldLengthValid(updatedProfile.Name, 50))
                return false;

            if (updatedProfile.UserId != userId)
                return false;

            _userProfiles[userId] = updatedProfile;
            _usedEmails.Add(updatedProfile.Email);

            return true;
        }

        public bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
            return emailRegex.IsMatch(email);
        }

        public bool IsFieldMandatory(string field)
        {
            return !string.IsNullOrWhiteSpace(field);
        }

        public bool IsFieldLengthValid(string field, int maxLength)
        {
            return field.Length <= maxLength;
        }

        public bool IsAuthorizedToUpdateProfile(string userId, string profileId)
        {
            return userId == profileId;
        }

        public bool UpdatePassword(string userId, string currentPassword, string newPassword)
        {
            if (!_userProfiles.ContainsKey(userId))
                return false;

            var profile = _userProfiles[userId];
            if (!string.Equals(profile.PasswordHash, currentPassword, StringComparison.Ordinal))
                return false;

            if (!IsPasswordStrong(newPassword))
                return false;

            profile.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            return true;
        }

        public bool UploadProfilePicture(string userId, byte[] picture)
        {
            if (!_userProfiles.ContainsKey(userId))
                return false;

            const int maxFileSize = 5 * 1024 * 1024; // 5 MB
            if (picture.Length > maxFileSize)
                return false;

            var profile = _userProfiles[userId];
            profile.ProfilePicture = picture;

            return true;
        }

        private bool IsPasswordStrong(string password)
        {
            if (password.Length < 8)
                return false;

            return Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
        }
    }
}
