using System;
using Xunit;
using Moq;
using SchoolApp.RoleManagement;
using SchoolApp.Models;
using System.Collections.Generic;
using SchoolApp.Repositories;

namespace TDDProjectDreamTeam.RoleManagement
{
    public class RoleManagementServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IRoleRepository> _mockRoleRepo;
        private readonly RoleManagementService _roleService;

        public RoleManagementServiceTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockRoleRepo = new Mock<IRoleRepository>();
            _roleService = new RoleManagementService(_mockUserRepo.Object, _mockRoleRepo.Object);
        }

        // Role Creation Tests
        [Fact]
        public void CreateRole_ValidRole_ShouldSucceed()
        {
            var role = new Role("Tutor", new List<string>());
            _mockRoleRepo.Setup(repo => repo.RoleExists(role.Name)).Returns(false);

            var result = _roleService.CreateRole(role);

            Assert.True(result, "Valid roles should be created successfully.");
            _mockRoleRepo.Verify(repo => repo.AddRole(role), Times.Once);
        }

        [Fact]
        public void CreateRole_DuplicateRole_ShouldFail()
        {
            var role = new Role("Staff", new List<string>());
            _mockRoleRepo.Setup(repo => repo.RoleExists(role.Name)).Returns(true);

            var result = _roleService.CreateRole(role);

            Assert.False(result, "Duplicate roles should not be allowed.");
            _mockRoleRepo.Verify(repo => repo.AddRole(It.IsAny<Role>()), Times.Never);
        }

        [Fact]
        public void CreateRole_EmptyName_ShouldThrowException()
        {
            var role = new Role("", new List<string>());

            var exception = Assert.Throws<ArgumentException>(() => _roleService.CreateRole(role));

            Assert.Equal("Role name cannot be empty.", exception.Message);
        }

        [Fact]
        public void CreateRole_NullRole_ShouldThrowException()
        {
            var exception = Assert.Throws<ArgumentException>(() => _roleService.CreateRole(null));
            Assert.Equal("Role name cannot be empty.", exception.Message);
        }

        // Role Assignment Tests
        [Fact]
        public void AssignRole_ValidRole_ShouldSucceed()
        {
            var role = new Role("Student", new List<string> { "Read" });
            var user = new User("12345", "Nat", null);

            _mockRoleRepo.Setup(repo => repo.RoleExists(role.Name)).Returns(true);
            _mockUserRepo.Setup(repo => repo.GetUser(user.Id)).Returns(user);

            var result = _roleService.AssignRole(user.Id, role);

            Assert.True(result, "Valid roles should be assigned successfully.");
            Assert.Equal(role, user.Role);
        }

        [Fact]
        public void AssignRole_InvalidRole_ShouldFail()
        {
            var role = new Role("NonExistent", null);
            var user = new User("12345", "Nat", null);

            _mockRoleRepo.Setup(repo => repo.RoleExists(role.Name)).Returns(false);
            _mockUserRepo.Setup(repo => repo.GetUser(user.Id)).Returns(user);

            var result = _roleService.AssignRole(user.Id, role);

            Assert.False(result, "Role assignment should fail for invalid roles.");
        }

        [Fact]
        public void AssignRole_EmptyUserId_ShouldThrowException()
        {
            var role = new Role("Student", new List<string> { "Read" });

            var exception = Assert.Throws<ArgumentException>(() => _roleService.AssignRole("", role));

            Assert.Equal("User ID cannot be empty.", exception.Message);
        }

        // Access Control Tests
        [Fact]
        public void HasAccess_AdminRole_ShouldHaveAllPermissions()
        {
            var role = new Role("Admin", new List<string> { "ModifyRoles", "ParkingAccess" });
            var user = new User("admin123", "John", role);

            _mockUserRepo.Setup(repo => repo.GetUser(user.Id)).Returns(user);

            foreach (var permission in role.Permissions)
            {
                Assert.True(_roleService.HasAccess(user.Id, permission), $"Admins should have access to {permission}.");
            }
        }

        [Fact]
        public void HasAccess_StaffRole_ShouldHaveLimitedPermissions()
        {
            var role = new Role("Staff", new List<string> { "ParkingAccess" });
            var user = new User("staff123", "Jane", role);

            _mockUserRepo.Setup(repo => repo.GetUser(user.Id)).Returns(user);

            Assert.True(_roleService.HasAccess(user.Id, "ParkingAccess"), "Staff should have access to ParkingAccess.");
            Assert.False(_roleService.HasAccess(user.Id, "ModifyRoles"), "Staff should not have access to ModifyRoles.");
        }

        // Role Update Tests
        [Fact]
        public void UpdateUserRole_StudentToStaff_ShouldSucceed()
        {
            var adminRole = new Role("Admin", new List<string> { "Create", "Read", "Update", "Delete" });
            var studentRole = new Role("Student", new List<string> { "Read" });
            var staffRole = new Role("Staff", new List<string> { "Read" });
            var adminUser = new User("admin123", "AdminUser", adminRole);
            var studentUser = new User("student123", "Alice", studentRole);

            _mockRoleRepo.Setup(repo => repo.RoleExists(It.IsAny<string>())).Returns(true);
            _mockUserRepo.Setup(repo => repo.GetUser(adminUser.Id)).Returns(adminUser);
            _mockUserRepo.Setup(repo => repo.GetUser(studentUser.Id)).Returns(studentUser);

            var result = _roleService.UpdateUserRole(adminUser.Id, studentUser.Id, staffRole);

            Assert.True(result, "Admin should be able to update their role to Staff.");
            Assert.Equal(staffRole, studentUser.Role);
        }

        [Fact]
        public void UpdateUserRole_LastAdmin_ShouldThrowException()
        {
            var adminRole = new Role("Admin", null);
            var adminUser = new User("admin123", "AdminUser", adminRole);

            _mockRoleRepo.Setup(repo => repo.RoleExists(It.IsAny<string>())).Returns(true);
            _mockUserRepo.Setup(repo => repo.GetUser(adminUser.Id)).Returns(adminUser);
            _mockUserRepo.Setup(repo => repo.GetUsers()).Returns(new List<User> { adminUser });

            var exception = Assert.Throws<InvalidOperationException>(() =>
                _roleService.UpdateUserRole(adminUser.Id, adminUser.Id, new Role("Staff", null)));

            Assert.Equal("Cannot update the last remaining admin to another role.", exception.Message);
        }

        [Fact]
        public void UpdateUserRole_NonAdmin_ShouldThrowException()
        {
            var staffRole = new Role("Staff", null);
            var nonAdminUser = new User("staff123", "Jane", staffRole);

            _mockRoleRepo.Setup(repo => repo.RoleExists(It.IsAny<string>())).Returns(true);
            _mockUserRepo.Setup(repo => repo.GetUser(nonAdminUser.Id)).Returns(nonAdminUser);

            var exception = Assert.Throws<UnauthorizedAccessException>(() =>
                _roleService.UpdateUserRole(nonAdminUser.Id, "user456", new Role("Student", null)));

            Assert.Equal("Only admins can update roles.", exception.Message);
        }

        [Fact]
        public void UpdateUserRole_NonExistentRole_ReturnsFalse()
        {
            var adminRole = new Role("Admin", null);
            var adminUser = new User("admin123", "AdminUser", adminRole);
            _mockRoleRepo.Setup(repo => repo.RoleExists(It.IsAny<string>())).Returns(false);
            _mockUserRepo.Setup(repo => repo.GetUser(adminUser.Id)).Returns(adminUser);
            var result = _roleService.UpdateUserRole(adminUser.Id, "user456", new Role("NonExistent", null));
            Assert.False(result, "Updating to a non-existent role should return false.");
        }

    }
}
