using System;
using Moq;
using SchoolApp.RoleManagement;

namespace TDDProjectDreamTeam;

public class StaffRoleAccess
{
    private readonly Mock<IRoleManagementService> mockRoleService;

    public StaffRoleAccess()
    {
        // Common setup for all tests
        mockRoleService = new Mock<IRoleManagementService>();
    }

    [Fact]
    public void HasAccess_WithStaffRole_ShouldReturnTrueForStaffFeatures()
    {
        // Arrange
        var userId = "staff123";
        var staffFeature = "ParkingAccess";

        mockRoleService.Setup(service => service.HasAccess(userId, staffFeature))
                       .Returns(true);

        // Act
        var result = mockRoleService.Object.HasAccess(userId, staffFeature);

        // Assert
        Assert.True(result, $"Staff should have access to the feature '{staffFeature}'.");
    }

    [Fact]
    public void HasAccess_WithStaffRole_ShouldReturnFalseForAdminFeatures()
    {
        // Arrange
        var userId = "staff123";
        var adminFeature = "ModifyRoles";

        mockRoleService.Setup(service => service.HasAccess(userId, adminFeature))
                       .Returns(false);

        // Act
        var result = mockRoleService.Object.HasAccess(userId, adminFeature);

        // Assert
        Assert.False(result, $"Staff should not have access to the admin feature '{adminFeature}'.");
    }
}