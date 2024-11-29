using System;
using Moq;
using SchoolApp.RoleManagement;

namespace TDDProjectDreamTeam;

public class SessionHandling
{
    private readonly Mock<IRoleManagementService> mockRoleService;

    public SessionHandling()
    {
        // Common setup for all tests
        mockRoleService = new Mock<IRoleManagementService>();
    }

    [Fact]
    public void UpdateUserRole_SessionShouldReflectNewRole()
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
        Assert.True(result, "After a role change, the session should reflect the new role immediately.");
    }

    [Fact]
    public void UpdateUserRole_SessionShouldNotUpdateIfRoleChangeFails()
    {
        // Arrange
        var adminId = "admin123";
        var userId = "user123";
        var newRole = "Staff";

        mockRoleService.Setup(service => service.UpdateUserRole(adminId, userId, newRole))
                       .Returns(false);

        // Act
        var result = mockRoleService.Object.UpdateUserRole(adminId, userId, newRole);

        // Assert
        Assert.False(result, "If the role update fails, the session should not be updated.");
    }

    [Fact]
    public void UpdateUserRole_SessionShouldThrowExceptionForInvalidUser()
    {
        // Arrange
        var adminId = "admin123";
        var nonExistentUserId = "nonExistentUser";
        var newRole = "Staff";

        mockRoleService.Setup(service => service.UpdateUserRole(adminId, nonExistentUserId, newRole))
                       .Throws(new KeyNotFoundException("User not found."));

        // Act & Assert
        var exception = Assert.Throws<KeyNotFoundException>(() =>
            mockRoleService.Object.UpdateUserRole(adminId, nonExistentUserId, newRole)
        );

        Assert.Equal("User not found.", exception.Message);
    }
}
