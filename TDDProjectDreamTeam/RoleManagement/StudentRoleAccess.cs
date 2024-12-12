using System;
using Moq;
using SchoolApp.RoleManagement;
using SchoolApp.Models;

namespace TDDProjectDreamTeam;

public class StudentRoleAccess
{
private readonly Mock<IRoleManagementService> mockRoleService;
private readonly RoleManagementService roleService;

    public StudentRoleAccess()
    {
        // Common setup for all tests
        mockRoleService = new Mock<IRoleManagementService>();
        roleService = new RoleManagementService();
    }

    [Fact]
    public void HasAccess_WithStudentRole_ShouldReturnTrueForStudentFeatures()
    {
        // Arrange
        var userId = "student123";
        var studentFeature = "LibraryAccess";
        var studentRole = new Role("Student", new List<string> { studentFeature });

        mockRoleService.Setup(service => service.HasAccess(userId, studentFeature))
                       .Returns(true);

        roleService.AddRole(studentRole);
        roleService.AddUser(new User("student123", "Jojo", studentRole));
       
        
        // Act
        var result = roleService.HasAccess(userId, studentFeature);

        // Assert
        Assert.True(result, $"Students should have access to the feature '{studentFeature}'.");
    }

    [Fact]
    public void HasAccess_WithStudentRole_ShouldReturnFalseForStaffFeatures()
    {
        // Arrange
        var userId = "student123";
        var staffFeature = "ParkingAccess";
        var staffRole = new Role("Staff", new List<string> { staffFeature });
        var studentRole = new Role("Student", new List<string>());

        roleService.AddRole(staffRole);
        roleService.AddRole(studentRole);
        roleService.AddUser(new User(userId, "Jojo", studentRole));

        mockRoleService.Setup(service => service.HasAccess(userId, staffFeature))
                       .Returns(false);

        // Act
        var result = roleService.HasAccess(userId, staffFeature);

        // Assert
        Assert.False(result, $"Students should not have access to the staff feature '{staffFeature}'.");
    }
}
