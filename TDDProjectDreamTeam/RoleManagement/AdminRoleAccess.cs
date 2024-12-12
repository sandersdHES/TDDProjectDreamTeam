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
        mockRoleService.Setup(service => service.HasAccess(It.Is<string>(id => id == "admin123"), It.IsAny<string>()))
                    .Returns(true);

        var features = new List<string>
        {
            "ModifyRoles",
            "ParkingAccess",
            "ViewReports"
        };

        roleService.AddRole(new Role("Admin", features));
        roleService.AddUser(new User("admin123", "John", "Admin"));

        // Act & Assert
        foreach (var feature in features)
        {
            var result = roleService.HasAccess("admin123", feature);
            Assert.True(result, $"Admins should have access to {feature}.");
        }
    }

    [Fact]
    public void UpdateUserRole_DeleteLastAdmin_ShouldReturnError()
    {
        // Arrange
        mockRoleService.Setup(service => service.UpdateUserRole(It.Is<string>(id => id == "admin123"), "lastAdminUser", null))
                    .Returns(false);

        //add admin role
        roleService.AddRole(new Role("Admin", new List<string> { "ModifyRoles", "ParkingAccess", "ViewReports" }));
        roleService.AddUser(new User("lastAdminUser", "John", "Admin"));
        roleService.AddUser(new User("admin123", "John", "Admin"));

        // Act
        var result = roleService.UpdateUserRole("admin123", "lastAdminUser", null);

        // Assert
        Assert.False(result, "The system should prevent deleting the last admin role.");
    }

}
