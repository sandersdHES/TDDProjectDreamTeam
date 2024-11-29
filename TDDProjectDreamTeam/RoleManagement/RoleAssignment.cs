using System;
using Moq;
using SchoolApp.RoleManagement;

namespace TDDProjectDreamTeam;

public class RoleAssignment
{
    private readonly Mock<IRoleManagementService> mockRoleService;

    public RoleAssignment(){

        // Common setup for all tests
        mockRoleService = new Mock<IRoleManagementService>();
    }
    [Fact]
    public void AssignRole_WithValidUserIdAndRoleName_ShouldReturnTrue()
    {
        
        // Arrange
        var userId = "12345";
        var roleName = "Student";

        mockRoleService.Setup(service => service.AssignRole(It.Is<string>(id => !string.IsNullOrWhiteSpace(id)),
                                                            It.Is<string>(role => !string.IsNullOrWhiteSpace(role))))
                    .Returns(true);

        // Act
        var result = mockRoleService.Object.AssignRole(userId, roleName);

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

        // Act
        var isValidRole = mockRoleService.Object.IsRoleValid(nonExistentRole);
        var result = mockRoleService.Object.AssignRole(userId, nonExistentRole);

        // Assert
        Assert.False(isValidRole, $"The role '{nonExistentRole}' should not be valid.");
        Assert.False(result, $"Assigning the non-existent role '{nonExistentRole}' to user '{userId}' should return false.");
    }

}
