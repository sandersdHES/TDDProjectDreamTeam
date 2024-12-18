using System.Collections.Generic;
using SchoolApp.Models;

namespace SchoolApp.Repositories
{
    public interface IUserRepository
    {
        User GetUser(string userId);
        void AddUser(User user);
        void RemoveUser(string userId);
        bool UserExists(string userId);
        IEnumerable<User> GetUsers();
        User GetUserByEmail(string email);
        void UpdateUser(User user);
    }
}