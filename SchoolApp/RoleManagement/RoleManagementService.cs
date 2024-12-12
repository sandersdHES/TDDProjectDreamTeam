using System;
using SchoolApp.RoleManagement.Models;

namespace SchoolApp.RoleManagement;

public class RoleManagementService : IRoleManagementService
{
    private readonly List<Role> roles; // List of defined roles
    private readonly List<User> users; // List of users in the system
    private readonly Action<string> logger;

    public RoleManagementService()
    {
        roles = new List<Role>();
        users = new List<User>();
        this.logger = logger ?? Console.WriteLine;
    }

    public bool CreateRole(string roleName)
    {
        ValidateInput(roleName, "Role name cannot be empty.");

        if (roles.Any(role => role.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase)))
            return false; // Role already exists

        roles.Add(new Role(roleName, new List<string>())); // Create a new role with no initial permissions
        logger?.Invoke($"Role '{roleName}' created.");
        return true;
    }

    public bool AssignRole(string userId, string roleName)
    {
         ValidateInput(userId, "User ID cannot be empty.");

        // Check if the role exists
        if (!IsRoleValid(roleName))
            return false; // Cannot assign a non-existent role

        // Find the user
        var user = GetUser(userId);

        // Assign the role to the user
        string oldRole = user.Role;
        user.Role = roleName;
        LogRoleChange(userId, oldRole, roleName); // Log the role change
        return true;
    }

    public bool HasAccess(string userId, string feature)
    {
        ValidateInput(userId, "User ID cannot be empty.");
        ValidateInput(feature, "Feature cannot be empty.");

        // Find the user
        var user = GetUser(userId);

        // Find the user's role
        var role = GetRole(user.Role);

        // Check if the role has access to the specified feature
        return role.HasPermission(feature);
    }

    public bool UpdateUserRole(string adminId, string userId, string newRole)
    {
        var admin = GetUser(adminId);
        if (!admin.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Only admins can update roles.");

            var user = GetUser(userId);

        if (!IsRoleValid(newRole))
        {
            return false; // New role does not exist
        }

        string oldRole = user.Role;
        user.Role = newRole;
        LogRoleChange(userId, oldRole, newRole); // Update session after role change
        return true;
    }

    public bool IsRoleValid(string roleName)
    {
        return roles.Any(role => role.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
    }

    public void LogRoleChange(string userId, string oldRole, string newRole)
    {
        logger?.Invoke($"User '{userId}' role changed from '{oldRole}' to '{newRole}'.");
    }

     private Role GetRole(string roleName) =>
            roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase))
            ?? throw new KeyNotFoundException($"Role '{roleName}' not found.");

    private User GetUser(string userId) =>
        users.FirstOrDefault(u => u.Id.Equals(userId, StringComparison.OrdinalIgnoreCase))
        ?? throw new KeyNotFoundException($"User '{userId}' not found.");

    public void AddUser(User user)
    {
        users.Add(user);
    }

    public void AddRole(Role role)
    {
        roles.Add(role);
    }

    private void ValidateInput(string input, string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException(errorMessage);
    }
}

