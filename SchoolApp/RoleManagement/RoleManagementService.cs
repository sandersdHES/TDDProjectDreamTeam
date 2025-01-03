using System;
using System.Collections.Generic;
using System.Linq;
using global::SchoolApp.Repositories;
using SchoolApp.Models;

namespace SchoolApp.RoleManagement;

public class RoleManagementService : IRoleManagementService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public RoleManagementService(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public bool CreateRole(Role role)
    {
        ValidateInput(role?.Name, "Role name cannot be empty.");

        if (_roleRepository.RoleExists(role.Name))
            return false; // Role already exists

        _roleRepository.AddRole(role);
        return true;
    }

    public bool AssignRole(string userId, Role role)
    {
        ValidateInput(userId, "User ID cannot be empty.");

        // Check if the role exists
        if (!_roleRepository.RoleExists(role.Name))
            return false; // Cannot assign a non-existent role

        // Find the user
        var user = _userRepository.GetUser(userId);

        // Assign the role to the user
        user.Role = role;
        return true;
    }

    public bool HasAccess(string userId, string feature)
    {
        ValidateInput(userId, "User ID cannot be empty.");
        ValidateInput(feature, "Feature cannot be empty.");

        // Find the user
        var user = _userRepository.GetUser(userId);

        // Check if the role has access to the specified feature
        return user.Role.HasPermission(feature);
    }

    public bool UpdateUserRole(string adminId, string userId, Role newRole)
    {
        var admin = _userRepository.GetUser(adminId);
        if (!admin.Role.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            throw new UnauthorizedAccessException("Only admins can update roles.");

        if (!_roleRepository.RoleExists(newRole.Name))
        {
            return false; // New role does not exist
        }

        var user = _userRepository.GetUser(userId);

        // Check if the user being updated is the only admin
        if (user.Role.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            //get all users and count them
            var adminCount = _userRepository.GetUsers()
                .Count(u => u.Role.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase));
            if (adminCount <= 1)
                throw new InvalidOperationException("Cannot update the last remaining admin to another role.");
        }

        user.Role = newRole;
        return true;
    }

    private void ValidateInput(string input, string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException(errorMessage);
    }
}


