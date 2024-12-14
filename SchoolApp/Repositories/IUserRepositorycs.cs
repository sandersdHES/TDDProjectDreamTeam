using SchoolApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

}
