using System;
using Moq;
using SchoolApp.RoleManagement;
using SchoolApp.Models;

namespace TDDProjectDreamTeam;

public class RoleUpdateByAdmin
{
    private readonly Mock<IRoleManagementService> mockRoleService;
    private readonly RoleManagementService roleService;

    public RoleUpdateByAdmin()
    {
        // Common setup for all tests
        mockRoleService = new Mock<IRoleManagementService>();
        roleService = new RoleManagementService();
    }
    [Fact]
    public void UpdateUserRole_Admin_ShouldUpdateAndReturnTrue()
    {
        // Arrange
        var adminId = "admin123";
        var userId = "user123";
        var newRole = new Role("Staff", new List<string> { "Read", "Update" });
        var adminRole = new Role("Admin", new List<string> { "Create", "Read", "Update", "Delete" });

        roleService.AddRole(adminRole);
        roleService.AddRole(newRole);
        roleService.AddUser(new User(adminId, "Admin User", adminRole));
        roleService.AddUser(new User(userId, "User One", new Role("Student", null)));

        mockRoleService.Setup(service => service.UpdateUserRole(adminId, userId, newRole))
                       .Returns(true);

        // Act
        var result = roleService.UpdateUserRole(adminId, userId, newRole);

        // Assert
        Assert.True(result, "Role update by an admin should be successful.");
    }

    [Fact]
    public void UpdateUserRole_NonAdmin_ShouldReturnFalse()
    {
        // Arrange
        var nonAdminId = "nonAdmin123";
        var userId = "user123";
        var newRole = new Role("Staff", new List<string> { "Read", "Update" });

        roleService.AddRole(new Role("Admin", new List<string>()));
        roleService.AddRole(newRole);
        roleService.AddUser(new User(nonAdminId, "Non-Admin User", new Role("Staff", null)));
        roleService.AddUser(new User(userId, "User One", new Role("Student", null)));

        mockRoleService.Setup(service => service.UpdateUserRole(nonAdminId, userId, newRole))
                       .Returns(false);

        // Act & Assert

        var exception = Assert.Throws<UnauthorizedAccessException>(() =>
            roleService.UpdateUserRole(nonAdminId, userId, newRole)
        );
        Assert.Equal("Only admins can update roles.", exception.Message);
    }

    //add test for checking that it returns false if new roll is not valid
    [Fact]
    public void UpdateUserRole_InvalidRole_ShouldReturnFalse()
    {
        // Arrange
        var adminId = "admin123";
        var userId = "user123";
        var invalidRole = new Role("InvalidRole", null);

        var adminRole = new Role("Admin", new List<string> { "Create", "Read", "Update", "Delete" });

        roleService.AddRole(adminRole);
        roleService.AddUser(new User(adminId, "Admin User", adminRole));

        mockRoleService.Setup(service => service.UpdateUserRole(adminId, userId, invalidRole))
            .Returns(false);

        // Act
        var result = roleService.UpdateUserRole(adminId, userId, invalidRole);

        // Assert
        Assert.False(result);

    }

}
