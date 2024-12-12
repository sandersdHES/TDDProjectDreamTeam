using SchoolApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp
{
    public interface IUserService
    {
        User RegisterUser(User user, string password);
        User AuthenticateUser(string email, string password);
        User GetUserById(int userId);
    }
}
