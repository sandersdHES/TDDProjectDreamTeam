using Moq;
using SchoolApp.RoleManagement;

namespace TDDProjectDreamTeam
{
    public class RoleCreation
    {
        [Fact]
public void CreateRole_WithValidName_ShouldReturnTrue()
{
    // Arrange
    var mockRoleService = new Mock<IRoleManagementService>();
    mockRoleService.Setup(service => service.CreateRole(It.Is<string>(name => !string.IsNullOrWhiteSpace(name))))
                   .Returns(true);

    // Act
    var result = mockRoleService.Object.CreateRole("Staff");

    // Assert
    Assert.True(result, "The role creation should return true for a valid role name.");
}

[Fact]
public void CreateRole_WithNoName_ShouldThrowArgumentException()
{
    // Arrange
    var mockRoleService = new Mock<IRoleManagementService>();
    mockRoleService.Setup(service => service.CreateRole(string.Empty))
                   .Throws(new ArgumentException("Role name cannot be empty."));

    // Act & Assert
    var exception = Assert.Throws<ArgumentException>(() => mockRoleService.Object.CreateRole(string.Empty));
    Assert.Equal("Role name cannot be empty.", exception.Message);
}

[Fact]
public void CreateRole_WithDuplicateName_ShouldReturnFalse()
{
    // Arrange
    var mockRoleService = new Mock<IRoleManagementService>();
    mockRoleService.Setup(service => service.CreateRole("Staff"))
                   .Returns(false); // Simulate that the role already exists

    // Act
    var result = mockRoleService.Object.CreateRole("Staff");

    // Assert
    Assert.False(result, "The role creation should return false if the role name already exists.");
}
    }
}