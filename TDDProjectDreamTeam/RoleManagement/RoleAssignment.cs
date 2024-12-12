using System;
using Moq;
using SchoolApp.RoleManagement;
using SchoolApp.RoleManagement.Models;

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
        var roleName = "Student";

        //add student role
        roleService.AddRole(new Role("Student", new List<string> { "Read" }));

        //add user with student role
        roleService.AddUser(new User(userId, "Nat", null));

        mockRoleService.Setup(service => service.AssignRole(It.Is<string>(id => !string.IsNullOrWhiteSpace(id)),
                                                            It.Is<string>(role => !string.IsNullOrWhiteSpace(role))))
                    .Returns(true);

        // Act
        var result = roleService.AssignRole(userId, "Student");

        // Assert
        Assert.True(result, $"Assigning the role '{roleName}' to user '{userId}' should return true.");
    }

    [Fact]
    public void AssignRole_WithNonExistentRole_ShouldReturnFalse()
    {
        // Arrange
        var userId = "12345";
        var nonExistentRole = "NonExistentRole";

        mockRoleService.Setup(service => service.IsRoleValid(nonExistentRole)).Returns(false);
        mockRoleService.Setup(service => service.AssignRole(userId, nonExistentRole)).Returns(false);

        //add user with student role
        roleService.AddUser(new User(userId, "Nat", null));

        // Act
        var isValidRole = roleService.IsRoleValid(nonExistentRole);
        var result = roleService.AssignRole(userId, nonExistentRole);

        // Assert
        Assert.False(isValidRole, $"The role '{nonExistentRole}' should not be valid.");
        Assert.False(result, $"Assigning the non-existent role '{nonExistentRole}' to user '{userId}' should return false.");
    }

}
