using System;
using SchoolApp.RoleManagement.Models;

namespace SchoolApp.RoleManagement;

public class RoleManagementService : IRoleManagementService
{
    private readonly List<Role> roles; // List of defined roles
    private readonly List<User> users; // List of users in the system

    public RoleManagementService()
    {
        roles = new List<Role>();
        users = new List<User>();
    }

    public bool CreateRole(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            throw new ArgumentException("Role name cannot be empty.");
        }

        if (roles.Any(role => role.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase)))
        {
            return false; // Role already exists
        }

        roles.Add(new Role(roleName, new List<string>())); // Create a new role with no initial permissions
        return true;
    }

    public bool AssignRole(string userId, string roleName)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("Role name cannot be empty.");
        }

        // Check if the role exists
        if (!IsRoleValid(roleName))
        {
            return false; // Cannot assign a non-existent role
        }

        // Find the user
        var user = users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        // Assign the role to the user
        user.Role = roleName;
        LogRoleChange(userId, user.Role, roleName); // Log the role change
        return true;
    }

    public bool HasAccess(string userId, string feature)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(feature))
        {
            throw new ArgumentException("User ID and feature cannot be null or empty.");
        }

        // Find the user
        var user = users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        // Find the user's role
        var role = roles.FirstOrDefault(r => r.Name.Equals(user.Role, StringComparison.OrdinalIgnoreCase));
        if (role == null)
        {
            return false; // No role assigned or role not found
        }

        // Check if the role has access to the specified feature
        return role.Permissions.Contains(feature, StringComparer.OrdinalIgnoreCase);
    }

    public bool UpdateUserRole(string adminId, string userId, string newRole)
    {
        var admin = users.FirstOrDefault(u => u.Id == adminId);
        if (admin == null || !admin.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException("Only admins can update user roles.");
        }

        var user = users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

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
        // Here we simply log the role change to the console for demonstration purposes.
        Console.WriteLine($"User '{userId}' role changed from '{oldRole}' to '{newRole}'.");
    }
}

