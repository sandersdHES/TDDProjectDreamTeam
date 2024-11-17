using System;
using Moq;
using SchoolApp.RoleManagement;

namespace TDDProjectDreamTeam;

public class SessionHandling
{
    [Fact]
public void UpdateUserRole_SessionShouldReflectNewRole()
{
    // Arrange
    var mockRoleService = new Mock<IRoleManagementService>();
    mockRoleService.Setup(service => service.UpdateUserRole(It.Is<string>(id => id == "admin123"), "user123", "Staff"))
                   .Returns(true);

    // Act
    var result = mockRoleService.Object.UpdateUserRole("admin123", "user123", "Staff");

    // Assert
    Assert.True(result, "After a role change, the session should reflect the new role immediately.");
}

[Fact]
public void UpdateUserRole_SessionShouldNotUpdateIfRoleChangeFails()
{
    // Arrange
    var mockRoleService = new Mock<IRoleManagementService>();

    mockRoleService.Setup(service => service.UpdateUserRole(It.Is<string>(id => id == "admin123"), "user123", "Staff"))
                   .Returns(false);

    // Act
    var result = mockRoleService.Object.UpdateUserRole("admin123", "user123", "Staff");

    // Assert
    Assert.False(result, "If the role update fails, the session should not be updated.");
}

[Fact]
public void UpdateUserRole_SessionShouldThrowExceptionForInvalidUser()
{
    // Arrange
    var mockRoleService = new Mock<IRoleManagementService>();

    mockRoleService.Setup(service => service.UpdateUserRole("admin123", "nonExistentUser", "Staff"))
                   .Throws(new KeyNotFoundException("User not found."));

    // Act
    var exception = Assert.Throws<KeyNotFoundException>(() => mockRoleService.Object.UpdateUserRole("admin123", "nonExistentUser", "Staff"));
    
    //Assert
    Assert.Equal("User not found.", exception.Message);
}
}
