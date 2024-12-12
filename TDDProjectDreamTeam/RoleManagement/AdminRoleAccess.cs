using System;
using Moq;
using SchoolApp.RoleManagement;
using SchoolApp.RoleManagement.Models;

namespace TDDProjectDreamTeam;

public class AdminRoleAccess
{
    private readonly Mock<IRoleManagementService> mockRoleService;
    private readonly RoleManagementService roleService;

    public AdminRoleAccess(){
         // Common setup for all tests
        mockRoleService = new Mock<IRoleManagementService>();
        roleService = new RoleManagementService();
    }
    [Fact]
    public void HasAccess_WithAdminRole_ShouldReturnTrueForAllFeatures()
    {
        // Arrange
        var adminRole = new Role("Admin", new List<string> { "ModifyRoles", "ParkingAccess", "ViewReports" });
        roleService.AddRole(adminRole);
        roleService.AddUser(new User("admin123", "John", adminRole));

        mockRoleService.Setup(service => service.HasAccess("admin123", It.IsAny<string>()))
                       .Returns(true);

        // Act & Assert
        foreach (var feature in adminRole.Permissions)
        {
            var result = roleService.HasAccess("admin123", feature);
            Assert.True(result, $"Admins should have access to {feature}.");
        }
    }

    [Fact]
    public void UpdateUserRole_DeleteLastAdmin_ShouldReturnError()
    {
        // Arrange
        var adminRole = new Role("Admin", null);
        var otherRole = new Role("Staff", null);
        roleService.AddRole(adminRole);
        roleService.AddRole(otherRole);
        roleService.AddUser(new User("admin123", "John", adminRole));

        mockRoleService.Setup(service => service.UpdateUserRole("admin123", "lastAdminUser", null))
                       .Returns(false);

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() =>
            roleService.UpdateUserRole("admin123", "admin123", otherRole)
        );

        // Assert
        Assert.Equal("Cannot update the last remaining admin to another role.", exception.Message);
    }

}
