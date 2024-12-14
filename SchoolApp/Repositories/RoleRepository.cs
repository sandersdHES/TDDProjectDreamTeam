using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolApp.Models;

namespace SchoolApp.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly List<Role> _roles = new List<Role>();

        public Role GetRole(string roleName)
        {
            return _roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase))
                   ?? throw new KeyNotFoundException($"Role '{roleName}' not found.");
        }

        public void AddRole(Role role)
        {
            if (_roles.Any(r => r.Name.Equals(role.Name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"Role '{role.Name}' already exists.");
            _roles.Add(role);
        }

        public void RemoveRole(string roleName)
        {
            var role = GetRole(roleName);
            _roles.Remove(role);
        }

        public bool RoleExists(string roleName)
        {
            return _roles.Any(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return _roles;
        }
    }

}
