using System;
using SchoolApp.Models;

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

    public bool CreateRole(Role role)
    {
        ValidateInput(role?.Name, "Role name cannot be empty.");

        if (roles.Any(r => r.Name.Equals(role.Name, StringComparison.OrdinalIgnoreCase)))
            return false; // Role already exists

        roles.Add(role);
        return true;
    }

    public bool AssignRole(string userId, Role role)
    {
        ValidateInput(userId, "User ID cannot be empty.");

        // Check if the role exists
        if (!IsRoleValid(role))
            return false; // Cannot assign a non-existent role

        // Find the user
        var user = GetUser(userId);

        // Assign the role to the user
        var oldRole = user.Role;
        user.Role = role;
        return true;
    }

    public bool HasAccess(string userId, string feature)
    {
        ValidateInput(userId, "User ID cannot be empty.");
        ValidateInput(feature, "Feature cannot be empty.");

        // Find the user
        var user = GetUser(userId);

        // Check if the role has access to the specified feature
        return user.Role.HasPermission(feature);
    }

    public bool UpdateUserRole(string adminId, string userId, Role newRole)
    {
        var admin = GetUser(adminId);
        if (!admin.Role.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Only admins can update roles.");

        if (!IsRoleValid(newRole))
        {
            return false; // New role does not exist
        }

        var user = GetUser(userId);

        // Check if the user being updated is the only admin
        if (user.Role.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            var adminCount = users.Count(u => u.Role.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase));
            if (adminCount <= 1)
                throw new InvalidOperationException("Cannot update the last remaining admin to another role.");
        }

        var oldRole = user.Role;
        user.Role = newRole;
        return true;
    }

    public bool IsRoleValid(Role role)
    {
        return roles.Any(r => r.Name.Equals(role.Name, StringComparison.OrdinalIgnoreCase));
    }

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

