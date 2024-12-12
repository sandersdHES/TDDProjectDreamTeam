using Moq;
using SchoolApp.RoleManagement;

namespace TDDProjectDreamTeam
{
    public class RoleCreation
    {
        private readonly Mock<IRoleManagementService> mockRoleService;
        private readonly RoleManagementService roleManagementService;

        public RoleCreation()
        {
            // Common setup for all tests
            mockRoleService = new Mock<IRoleManagementService>();
            roleManagementService = new RoleManagementService();
        }
        [Fact]
        public void CreateRole_WithValidName_ShouldReturnTrue()
        {
            // Arrange
            var roleName = "Tutor";
            mockRoleService.Setup(service => service.CreateRole(It.Is<string>(name => !string.IsNullOrWhiteSpace(name))))
                           .Returns(true);

            // Act
            var result = roleManagementService.CreateRole(roleName);

            // Assert
            Assert.True(result, $"The role creation should return true for a valid role name '{roleName}'.");
        }

        [Fact]
        public void CreateRole_WithNoName_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidRoleName = string.Empty;
            mockRoleService.Setup(service => service.CreateRole(invalidRoleName))
                           .Throws(new ArgumentException("Role name cannot be empty."));

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => roleManagementService.CreateRole(invalidRoleName));
            Assert.Equal("Role name cannot be empty.", exception.Message);
        }

        [Fact]
        public void CreateRole_WithDuplicateName_ShouldReturnFalse()
        {
            // Arrange
            var duplicateRoleName = "Staff";
            mockRoleService.Setup(service => service.CreateRole(duplicateRoleName))
                           .Returns(false); // Simulate that the role already exists
            // Add the duplicate role to the list of roles
            roleManagementService.CreateRole(duplicateRoleName);

            // Act
            var result = roleManagementService.CreateRole(duplicateRoleName);

            // Assert
            Assert.False(result, $"The role creation should return false if the role name '{duplicateRoleName}' already exists.");
        }
    }
}