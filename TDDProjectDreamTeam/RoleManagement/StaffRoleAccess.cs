using System;
using Moq;
using SchoolApp.RoleManagement;
using SchoolApp.RoleManagement.Models;

namespace TDDProjectDreamTeam;

public class StaffRoleAccess
{
    private readonly Mock<IRoleManagementService> mockRoleService;
    private readonly RoleManagementService roleService;

    public StaffRoleAccess()
    {
        // Common setup for all tests
        mockRoleService = new Mock<IRoleManagementService>();
        roleService = new RoleManagementService();
    }

    [Fact]
    public void HasAccess_WithStaffRole_ShouldReturnTrueForStaffFeatures()
    {
        // Arrange
        var userId = "staff123";
        var staffFeature = "ParkingAccess";
        var permissions = new List<String>
        {
            staffFeature
        };

        mockRoleService.Setup(service => service.HasAccess(userId, staffFeature))
                       .Returns(true);

        roleService.AddRole(new Role("Staff", permissions));
        roleService.AddUser(new User("staff123", "John", "Staff"));

        // Act
        var result = roleService.HasAccess(userId, staffFeature);

        // Assert
        Assert.True(result, $"Staff should have access to the feature '{staffFeature}'.");
    }

    [Fact]
    public void HasAccess_WithStaffRole_ShouldReturnFalseForAdminFeatures()
    {
        // Arrange
        var userId = "staff123";
        var adminFeature = "ModifyRoles";
        var permissions = new List<String>
        {
            "ParkingAccess"
        };

        mockRoleService.Setup(service => service.HasAccess(userId, adminFeature))
                       .Returns(false);

        roleService.AddRole(new Role("Staff", permissions));
        roleService.AddUser(new User("staff123", "John", "Staff"));

        // Act
        var result = roleService.HasAccess(userId, adminFeature);

        // Assert
        Assert.False(result, $"Staff should not have access to the admin feature '{adminFeature}'.");
    }
}