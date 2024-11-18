using System;
using Moq;
using SchoolApp.RoleManagement;

namespace TDDProjectDreamTeam;

public class RoleAssignment
{
    [Fact]
public void AssignRole_WithValidUserIdAndRoleName_ShouldReturnTrue()
{
    // Arrange
    var mockRoleService = new Mock<IRoleManagementService>();
    mockRoleService.Setup(service => service.AssignRole(It.Is<string>(id => !string.IsNullOrWhiteSpace(id)), 
                                                        It.Is<string>(role => !string.IsNullOrWhiteSpace(role))))
                   .Returns(true);

    // Act
    var result = mockRoleService.Object.AssignRole("12345", "Student");

    // Assert
    Assert.True(result, "Assigning a valid role to a valid user ID should return true.");
}

[Fact]
public void AssignRole_WithNonExistentRole_ShouldReturnFalse()
{
    // Arrange
    var mockRoleService = new Mock<IRoleManagementService>();
    mockRoleService.Setup(service => service.IsRoleValid("NonExistentRole"))
                   .Returns(false);
    mockRoleService.Setup(service => service.AssignRole("12345", "NonExistentRole"))
                   .Returns(false);

    // Act
    var isValidRole = mockRoleService.Object.IsRoleValid("NonExistentRole");
    var result = isValidRole ? mockRoleService.Object.AssignRole("12345", "NonExistentRole") : false;

    // Assert
    Assert.False(result, "Assigning a non-existent role should return false.");
    Assert.False(isValidRole, "The role validity check should return false for a non-existent role.");
}

}
