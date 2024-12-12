using System;
using SchoolApp.RoleManagement.Models;

namespace SchoolApp.RoleManagement;

public interface IRoleManagementService
{
    bool CreateRole(Role role);
    bool AssignRole(string userId, Role role);
    bool IsRoleValid(Role role);
    bool HasAccess(string userId, string feature);
    bool UpdateUserRole(string adminId, string userId, Role newRole);
}
