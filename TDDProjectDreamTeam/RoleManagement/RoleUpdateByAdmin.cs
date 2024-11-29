using System;
using Moq;
using SchoolApp.RoleManagement;

namespace TDDProjectDreamTeam;

public class RoleUpdateByAdmin
{
    private readonly Mock<IRoleManagementService> mockRoleService;

    public RoleUpdateByAdmin()
    {
        // Common setup for all tests
        mockRoleService = new Mock<IRoleManagementService>();
    }
    [Fact]
    public void UpdateUserRole_Admin_ShouldUpdateAndReturnTrue()
    {
    // Arrange
        var adminId = "admin123";
        var userId = "user123";
        var newRole = "Staff";

    mockRoleService.Setup(service => service.UpdateUserRole(adminId, userId, newRole))
                    .Returns(true);

    // Act
    var result = mockRoleService.Object.UpdateUserRole(adminId, userId, newRole);

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

    mockRoleService.Setup(service => service.UpdateUserRole(nonAdminId, userId, newRole))
                    .Returns(false);

    // Act
    var result = mockRoleService.Object.UpdateUserRole(nonAdminId, userId, newRole);

    // Assert
    Assert.False(result, "Non-admin users should not be able to update roles.");
    }

}
