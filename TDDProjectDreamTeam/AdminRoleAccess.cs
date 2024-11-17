using System;
using Moq;
using SchoolApp.RoleManagement;

namespace TDDProjectDreamTeam;

public class AdminRoleAccess
{
    [Fact]
public void HasAccess_WithAdminRole_ShouldReturnTrueForAllFeatures()
{
    // Arrange
    var mockRoleService = new Mock<IRoleManagementService>();
    mockRoleService.Setup(service => service.HasAccess(It.Is<string>(id => id == "admin123"), It.IsAny<string>()))
                   .Returns(true);

    // Act
    var result1 = mockRoleService.Object.HasAccess("admin123", "ModifyRoles");
    var result2 = mockRoleService.Object.HasAccess("admin123", "ViewReports");
    var result3 = mockRoleService.Object.HasAccess("admin123", "DeleteUser");

    // Assert
    Assert.True(result1, "Admins should have access to ModifyRoles.");
    Assert.True(result2, "Admins should have access to ViewReports.");
    Assert.True(result3, "Admins should have access to DeleteUser.");
}

[Fact]
public void UpdateUserRole_DeleteLastAdmin_ShouldReturnError()
{
    // Arrange
    var mockRoleService = new Mock<IRoleManagementService>();
    mockRoleService.Setup(service => service.UpdateUserRole(It.Is<string>(id => id == "admin123"), "lastAdminUser", null))
                   .Returns(false);

    // Act
    var result = mockRoleService.Object.UpdateUserRole("admin123", "lastAdminUser", null);

    // Assert
    Assert.False(result, "The system should prevent deleting the last admin role.");
}

}
