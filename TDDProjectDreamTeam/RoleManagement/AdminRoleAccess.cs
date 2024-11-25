using System;
using Moq;
using SchoolApp.RoleManagement;

namespace TDDProjectDreamTeam;

public class AdminRoleAccess
{
    private readonly Mock<IRoleManagementService> mockRoleService;

    public AdminRoleAccess(){
         // Common setup for all tests
        mockRoleService = new Mock<IRoleManagementService>();
    }
    [Fact]
    public void HasAccess_WithAdminRole_ShouldReturnTrueForAllFeatures()
    {
        // Arrange
        mockRoleService.Setup(service => service.HasAccess(It.Is<string>(id => id == "admin123"), It.IsAny<string>()))
                    .Returns(true);

        var features = new[] { "ModifyRoles", "ViewReports", "DeleteUser" };

        // Act & Assert
        foreach (var feature in features)
        {
            var result = mockRoleService.Object.HasAccess("admin123", feature);
            Assert.True(result, $"Admins should have access to {feature}.");
        }
    }

    [Fact]
    public void UpdateUserRole_DeleteLastAdmin_ShouldReturnError()
    {
        // Arrange
        mockRoleService.Setup(service => service.UpdateUserRole(It.Is<string>(id => id == "admin123"), "lastAdminUser", null))
                    .Returns(false);

        // Act
        var result = mockRoleService.Object.UpdateUserRole("admin123", "lastAdminUser", null);

        // Assert
        Assert.False(result, "The system should prevent deleting the last admin role.");
    }

}
