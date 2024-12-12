using Moq;
using SchoolApp.RoleManagement;
using SchoolApp.RoleManagement.Models;

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
            var role = new Role("Tutor", new List<string>());
            mockRoleService.Setup(service => service.CreateRole(role))
                           .Returns(true);

            // Act
            var result = roleManagementService.CreateRole(role);

            // Assert
            Assert.True(result, $"The role creation should return true for a valid role '{role.Name}'.");
        }

        [Fact]
        public void CreateRole_WithNoName_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidRoleName = string.Empty;
            var invalidRole = new Role(invalidRoleName, new List<string>());
            mockRoleService.Setup(service => service.CreateRole(invalidRole))
                           .Throws(new ArgumentException("Role name cannot be empty."));

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => roleManagementService.CreateRole(invalidRole));
            Assert.Equal("Role name cannot be empty.", exception.Message);
        }

        [Fact]
        public void CreateRole_WithDuplicateName_ShouldReturnFalse()
        {
            // Arrange
            var duplicateRole = new Role("Staff", new List<string>());
            mockRoleService.Setup(service => service.CreateRole(duplicateRole))
                           .Returns(false); // Simulate that the role already exists

            roleManagementService.CreateRole(duplicateRole);

            // Act
            var result = roleManagementService.CreateRole(duplicateRole);

            // Assert
            Assert.False(result, $"The role creation should return false if the role '{duplicateRole.Name}' already exists.");
        }
    }
}