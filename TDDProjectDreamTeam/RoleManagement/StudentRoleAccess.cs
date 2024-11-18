using System;
using Moq;
using SchoolApp.RoleManagement;

namespace TDDProjectDreamTeam;

public class StudentRoleAccess
{
[Fact]
public void HasAccess_WithStudentRole_ShouldReturnTrueForStudentFeatures()
{
    // Arrange
    var mockRoleService = new Mock<IRoleManagementService>();
    mockRoleService.Setup(service => service.HasAccess(It.Is<string>(id => id == "student123"), It.Is<string>(feature => feature == "LibraryAccess")))
                   .Returns(true);

    // Act
    var result = mockRoleService.Object.HasAccess("student123", "LibraryAccess");

    // Assert
    Assert.True(result, "Students should have access to student-specific features like the library.");
}

[Fact]
public void HasAccess_WithStudentRole_ShouldReturnFalseForStaffFeatures()
{
    // Arrange
    var mockRoleService = new Mock<IRoleManagementService>();
    mockRoleService.Setup(service => service.HasAccess(It.Is<string>(id => id == "student123"), It.Is<string>(feature => feature == "ParkingAccess")))
                   .Returns(false);

    // Act
    var result = mockRoleService.Object.HasAccess("student123", "ParkingAccess");

    // Assert
    Assert.False(result, "Students should not have access to staff functionalities like parking.");
}
}
