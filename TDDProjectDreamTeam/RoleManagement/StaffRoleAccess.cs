using System;
using Moq;
using SchoolApp.RoleManagement;

namespace TDDProjectDreamTeam;

public class StaffRoleAccess
{
    [Fact]
public void HasAccess_WithStaffRole_ShouldReturnTrueForStaffFeatures()
{
    // Arrange
    var mockRoleService = new Mock<IRoleManagementService>();
    mockRoleService.Setup(service => service.HasAccess(It.Is<string>(id => id == "staff123"), It.Is<string>(feature => feature == "ParkingAccess")))
                   .Returns(true);

    // Act
    var result = mockRoleService.Object.HasAccess("staff123", "ParkingAccess");

    // Assert
    Assert.True(result, "Staff should have access to staff-only features like parking.");
}

[Fact]
public void HasAccess_WithStaffRole_ShouldReturnFalseForAdminFeatures()
{
    // Arrange
    var mockRoleService = new Mock<IRoleManagementService>();
    mockRoleService.Setup(service => service.HasAccess(It.Is<string>(id => id == "staff123"), It.Is<string>(feature => feature == "ModifyRoles")))
                   .Returns(false);

    // Act
    var result = mockRoleService.Object.HasAccess("staff123", "ModifyRoles");

    // Assert
    Assert.False(result, "Staff should not have access to admin functionalities.");
}
}
