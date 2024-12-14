using SchoolApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Repositories
{
    public interface IRoleRepository
    {
        Role GetRole(string roleName);
        void AddRole(Role role);
        void RemoveRole(string roleName);
        bool RoleExists(string roleName);
        IEnumerable<Role> GetAllRoles();
    }

}
