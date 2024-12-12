using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.UserAuthentication.Models
{
    public class AuthService : IAuthService
    {
        private readonly Dictionary<string, (string HashedPassword, int FailedAttempts, bool IsLocked)> _users;
        private readonly IHashingService _hashingService;
        private const int MaxFailedAttempts = 5;

        public AuthService(IHashingService hashingService)
        {
            _hashingService = hashingService;

            // Simulated user database
            _users = new Dictionary<string, (string HashedPassword, int FailedAttempts, bool IsLocked)>
            {
                { "user@example.com", (_hashingService.HashPassword("Password123!"), 0, false) }
            };
        }

        public async Task<bool> AuthenticateAsync(string email, string password)
        {
            if (!_users.ContainsKey(email))
            {
                return false;
            }

            var user = _users[email];

            if (user.IsLocked)
            {
                return false;
            }

            var isValid = _hashingService.VerifyPassword(password, user.HashedPassword);

            if (!isValid)
            {
                _users[email] = (user.HashedPassword, user.FailedAttempts + 1, user.FailedAttempts + 1 >= MaxFailedAttempts);
            }

            return await Task.FromResult(isValid);
        }

        public async Task<bool> IsAccountLockedAsync(string email)
        {
            if (!_users.ContainsKey(email))
            {
                return false;
            }

            return await Task.FromResult(_users[email].IsLocked);
        }

        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            if (!_users.ContainsKey(email))
            {
                return false;
            }

            var user = _users[email];
            _users[email] = (_hashingService.HashPassword(newPassword), 0, false);

            return await Task.FromResult(true);
        }
    }

}
