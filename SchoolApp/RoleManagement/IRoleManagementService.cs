using System;
using SchoolApp.Models;

namespace SchoolApp.RoleManagement;

public interface IRoleManagementService
{
    bool CreateRole(Role role);
    bool AssignRole(string userId, Role role);
    bool HasAccess(string userId, string feature);
    bool UpdateUserRole(string adminId, string userId, Role newRole);
}
