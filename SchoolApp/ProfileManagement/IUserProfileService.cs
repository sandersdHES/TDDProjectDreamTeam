using SchoolApp.Models;

namespace SchoolApp.ProfileManagement
{
    public interface IUserProfileService
    {
        User GetProfile(string userId);
        bool UpdateProfile(string userId, User updatedUser);
        bool UpdatePassword(string userId, string currentPassword, string newPassword);
        bool IsAuthorizedToUpdateProfile(string userId, string profileId);
    }
}
