using System;
using Moq;
using SchoolApp.RoleManagement;
using SchoolApp.RoleManagement.Models;

namespace TDDProjectDreamTeam;

public class SessionHandling
{
    private readonly Mock<IRoleManagementService> mockRoleService;
    private readonly RoleManagementService roleService;

    public SessionHandling()
    {
        // Common setup for all tests
        mockRoleService = new Mock<IRoleManagementService>();
        roleService = new RoleManagementService();
    }

    [Fact]
    public void UpdateUserRole_SessionShouldReflectNewRole()
    {
        // Arrange
        var adminId = "admin123";
        var userId = "user123";
        var newRole = new Role("Staff", null);
        var adminRole = new Role("Admin", null);

        roleService.AddRole(adminRole);
        roleService.AddRole(newRole);
        roleService.AddUser(new User(adminId, "John", adminRole));
        roleService.AddUser(new User(userId, "Jane", new Role("Student", null)));

        mockRoleService.Setup(service => service.UpdateUserRole(adminId, userId, newRole))
                       .Returns(true);

        // Act
        var result = roleService.UpdateUserRole(adminId, userId, newRole);

        // Assert
        Assert.True(result, "After a role change, the session should reflect the new role immediately.");
    }

    [Fact]
    public void UpdateUserRole_SessionShouldNotUpdateIfRoleChangeFails()
    {
        // Arrange
        var adminId = "admin123";
        var userId = "user123";
        var adminRole = new Role("Admin", null);
        var staffRole = new Role("Staff", null);
        var newRole = new Role("Student", null);

        mockRoleService.Setup(service => service.UpdateUserRole(adminId, userId, staffRole))
                       .Returns(false);

        //create admin and staff role
        roleService.AddRole(adminRole);
        roleService.AddRole(staffRole);

        //create admin and random user
        roleService.AddUser(new User(adminId, "John", adminRole));
        roleService.AddUser(new User(userId, "Jane", staffRole));

        // Act
        var result = mockRoleService.Object.UpdateUserRole(adminId, userId, newRole);

        // Assert
        Assert.False(result, "If the role update fails, the session should not be updated.");
    }

    [Fact]
    public void UpdateUserRole_SessionShouldThrowExceptionForInvalidUser()
    {
        // Arrange
        var adminId = "admin123";
        var nonExistentUserId = "nonExistentUser";
        var newRole = new Role("Staff", null);

        roleService.AddRole(new Role("Admin", null));
        roleService.AddRole(newRole);
        roleService.AddUser(new User(adminId, "John", new Role("Admin", null)));

        mockRoleService.Setup(service => service.UpdateUserRole(adminId, nonExistentUserId, newRole))
                       .Throws(new KeyNotFoundException("User not found."));

        // Act & Assert
        var exception = Assert.Throws<KeyNotFoundException>(() =>
            roleService.UpdateUserRole(adminId, nonExistentUserId, newRole)
        );

        Assert.Equal("User 'nonExistentUser' not found.", exception.Message);
    }
}
