using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolApp.Models;

namespace SchoolApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        public User GetUser(string userId)
        {
            return _users.FirstOrDefault(u => u.Id == userId)
                   ?? throw new KeyNotFoundException($"User '{userId}' not found.");
        }

        public void AddUser(User user)
        {
            if (_users.Any(u => u.Id == user.Id))
                throw new InvalidOperationException($"User '{user.Id}' already exists.");
            _users.Add(user);
        }

        public void RemoveUser(string userId)
        {
            var user = GetUser(userId);
            _users.Remove(user);
        }

        public bool UserExists(string userId)
        {
            return _users.Any(u => u.Id == userId);
        }

        public IEnumerable<User> GetUsers() {
            return _users;
        }

        public User GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email)
                   ?? throw new KeyNotFoundException($"User with email '{email}' not found.");
        }
    }

}
