using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.ProfileManagement.Models
{
    public class UserProfile
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public byte[] ProfilePicture { get; set; } // For profile pictures
    }
}
