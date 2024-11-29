using System;
using Moq;
using SchoolApp.RoleManagement;

namespace TDDProjectDreamTeam;

public class StudentRoleAccess
{
private readonly Mock<IRoleManagementService> mockRoleService;

    public StudentRoleAccess()
    {
        // Common setup for all tests
        mockRoleService = new Mock<IRoleManagementService>();
    }

    [Fact]
    public void HasAccess_WithStudentRole_ShouldReturnTrueForStudentFeatures()
    {
        // Arrange
        var userId = "student123";
        var studentFeature = "LibraryAccess";

        mockRoleService.Setup(service => service.HasAccess(userId, studentFeature))
                       .Returns(true);

        // Act
        var result = mockRoleService.Object.HasAccess(userId, studentFeature);

        // Assert
        Assert.True(result, $"Students should have access to the feature '{studentFeature}'.");
    }

    [Fact]
    public void HasAccess_WithStudentRole_ShouldReturnFalseForStaffFeatures()
    {
        // Arrange
        var userId = "student123";
        var staffFeature = "ParkingAccess";

        mockRoleService.Setup(service => service.HasAccess(userId, staffFeature))
                       .Returns(false);

        // Act
        var result = mockRoleService.Object.HasAccess(userId, staffFeature);

        // Assert
        Assert.False(result, $"Students should not have access to the staff feature '{staffFeature}'.");
    }
}
