using System;
using Moq;
using SchoolApp.RoleManagement;

namespace TDDProjectDreamTeam;

public class RoleUpdateByAdmin
{
    [Fact]
public void UpdateUserRole_Admin_ShouldUpdateAndReturnTrue()
{
    // Arrange
    var mockRoleService = new Mock<IRoleManagementService>();
    mockRoleService.Setup(service => service.UpdateUserRole("admin123", "user123", "Staff")).Returns(true);

    // Act
    var result = mockRoleService.Object.UpdateUserRole("admin123", "user123", "Staff");

    // Assert
    Assert.True(result, "Role update by an admin should be successful.");
}

[Fact]
public void UpdateUserRole_NonAdmin_ShouldReturnFalse()
{
    // Arrange
    var mockRoleService = new Mock<IRoleManagementService>();
    mockRoleService.Setup(service => service.UpdateUserRole(It.Is<string>(id => id != "admin123"), "user123", "Staff"))
                   .Returns(false);

    // Act
    var result = mockRoleService.Object.UpdateUserRole("nonAdmin123", "user123", "Staff");

    // Assert
    Assert.False(result, "Non-admin users should not be able to update roles.");
}

}
