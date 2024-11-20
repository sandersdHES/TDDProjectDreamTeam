using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.ProfileManagement
{
    public interface IUserProfileService
    {
        UserProfile GetProfile(string userId);
        bool UpdateProfile(string userId, UserProfile updatedProfile);
        bool IsEmailValid(string email);
        bool IsFieldMandatory(string field);
        bool IsFieldLengthValid(string field, int maxLength);
        bool IsAuthorizedToUpdateProfile(string userId, string profileId);
        bool UpdatePassword(string userId, string currentPassword, string newPassword);
        bool UploadProfilePicture(string userId, byte[] picture);
    }

    public class UserProfile
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        // Additional profile fields
    }
}
