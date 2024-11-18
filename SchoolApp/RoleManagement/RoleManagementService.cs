using System;

namespace SchoolApp.RoleManagement;

public class RoleManagementService : IRoleManagementService
{
    private readonly HashSet<string> roles;

    public RoleManagementService()
    {
        roles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }

    public bool CreateRole(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            throw new ArgumentException("Role name cannot be null or empty.");
        }

        if (roles.Contains(roleName))
        {
            return false; // Role already exists
        }

        roles.Add(roleName);
        return true; // Successfully added the new role
    }

    public bool AssignRole(string userId, string roleName)
    {
        // This method is not relevant for the current test, so it can remain unimplemented for now.
        throw new NotImplementedException();
    }

    public bool HasAccess(string userId, string feature)
    {
        // This method is not relevant for the current test, so it can remain unimplemented for now.
        throw new NotImplementedException();
    }

    public bool UpdateUserRole(string adminId, string userId, string newRole)
    {
        // This method is not relevant for the current test, so it can remain unimplemented for now.
        throw new NotImplementedException();
    }

    public bool IsRoleValid(string roleName)
    {
        throw new NotImplementedException();
    }

    public void LogRoleChange(string userId, string oldRole, string newRole)
    {
        throw new NotImplementedException();
    }
}

