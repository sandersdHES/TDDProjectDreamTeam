using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.UserAuthentication
{
    public interface IAuthService
    {
        Task<bool> AuthenticateAsync(string email, string password);
        Task<bool> IsAccountLockedAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string newPassword);
    }

}
