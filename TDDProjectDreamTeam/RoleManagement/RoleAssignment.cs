using System;
using Moq;
using SchoolApp.RoleManagement;
using SchoolApp.RoleManagement.Models;
using SchoolApp.RoleManagement.Models; // Assuming Role class is in this namespace

namespace TDDProjectDreamTeam;

public class RoleAssignment
{
    private readonly Mock<IRoleManagementService> mockRoleService;
    private readonly RoleManagementService roleService;

    public RoleAssignment(){

        // Common setup for all tests
        mockRoleService = new Mock<IRoleManagementService>();
        roleService = new RoleManagementService();
    }
    [Fact]
    public void AssignRole_WithValidUserIdAndRoleName_ShouldReturnTrue()
    {
        // Arrange
        var userId = "12345";
        var role = new Role("Student", new List<string> { "Read" });
        mockRoleService.Setup(service => service.AssignRole(userId, role))
                       .Returns(true);

        roleService.AddRole(role);
        roleService.AddUser(new User(userId, "Nat", null));

        // Act
        var result = roleService.AssignRole(userId, role);

        // Assert
        Assert.True(result, $"Assigning the role '{role.Name}' to user '{userId}' should return true.");
    }

    [Fact]
    public void AssignRole_WithNonExistentRole_ShouldReturnFalse()
    {
        // Arrange
        var userId = "12345";
        var nonExistentRole = new Role("NonExistentRole", null);
        mockRoleService.Setup(service => service.IsRoleValid(nonExistentRole)).Returns(false);
        mockRoleService.Setup(service => service.AssignRole(userId, nonExistentRole)).Returns(false);

        roleService.AddUser(new User(userId, "Nat", null));

        // Act
        var isValidRole = roleService.IsRoleValid(nonExistentRole);
        var result = roleService.AssignRole(userId, nonExistentRole);

        // Assert
        Assert.False(isValidRole, $"The role '{nonExistentRole.Name}' should not be valid.");
        Assert.False(result, $"Assigning the non-existent role '{nonExistentRole.Name}' to user '{userId}' should return false.");
    }

}
