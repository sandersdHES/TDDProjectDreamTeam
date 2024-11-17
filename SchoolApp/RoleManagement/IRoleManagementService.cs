using System;

namespace SchoolApp.RoleManagement;

public interface IRoleManagementService
{
    bool CreateRole(string roleName);
    bool AssignRole(string userId, string roleName);
    bool IsRoleValid(string roleName);
    bool HasAccess(string userId, string feature);
    bool UpdateUserRole(string adminId, string userId, string newRoleName);
    void LogRoleChange(string userId, string oldRole, string newRole);
}
