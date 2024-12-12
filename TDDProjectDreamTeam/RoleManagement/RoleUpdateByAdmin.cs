using System;
using Moq;
using SchoolApp.RoleManagement;
using SchoolApp.RoleManagement.Models;

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
        var newRole = "Staff";

        //create admin and staff role
        roleService.AddRole
            (
            new Role("Admin", new List<string> { "Create", "Read", "Update", "Delete" })
        );
        roleService.AddRole
            (
            new Role("Staff", new List<string> { "Read", "Update" })
        );
        //add admin user
        roleService.AddUser(new User("admin123", "Admin User", "Admin"));

        // add user with student role
        roleService.AddUser(new User("user123", "User One", "Student"));

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
    var newRole = "Staff";

        //Add admin and staff role
        roleService.AddRole
            (
            new Role("Admin", new List<string> { "Create", "Read", "Update", "Delete" })
        );
        roleService.AddRole
            (
            new Role("Staff", new List<string> { "Read", "Update" })
        );

        // Add non-admin user
        roleService.AddUser(new User("nonAdmin123", "Non-Admin User", "Staff"));

        //add a student user
        roleService.AddUser(new User("user123", "User One", "Student"));

        mockRoleService.Setup(service => service.UpdateUserRole(nonAdminId, userId, newRole))
                    .Returns(false);

        // Act
        //should throw exception
        var exception = Assert.Throws<UnauthorizedAccessException>(() =>
            roleService.UpdateUserRole(nonAdminId, userId, newRole)
        );
        // Assert
        Assert.Equal("Only admins can update roles.", exception.Message);
    }

}
