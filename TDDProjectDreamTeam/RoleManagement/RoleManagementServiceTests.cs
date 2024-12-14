using System;
using Xunit;
using SchoolApp.RoleManagement;
using SchoolApp.Models;

namespace TDDProjectDreamTeam.RoleManagement
{
    public class RoleManagementServiceTests
    {
        private readonly RoleManagementService roleService;

        public RoleManagementServiceTests()
        {
            roleService = new RoleManagementService();
        }

        // Role Creation Tests
        [Fact]
        public void CreateRole_ValidRole_ShouldSucceed()
        {
            var role = new Role("Tutor", new List<string>());
            var result = roleService.CreateRole(role);
            Assert.True(result, "Valid roles should be created successfully.");
        }

        [Fact]
        public void CreateRole_DuplicateRole_ShouldFail()
        {
            var role = new Role("Staff", new List<string>());
            roleService.CreateRole(role);
            var result = roleService.CreateRole(role);
            Assert.False(result, "Duplicate roles should not be allowed.");
        }

        [Fact]
        public void CreateRole_EmptyName_ShouldThrowException()
        {
            var role = new Role("", new List<string>());
            var exception = Assert.Throws<ArgumentException>(() => roleService.CreateRole(role));
            Assert.Equal("Role name cannot be empty.", exception.Message);
        }

        // Role Assignment Tests
        [Fact]
        public void AssignRole_ValidRole_ShouldSucceed()
        {
            var role = new Role("Student", new List<string> { "Read" });
            var user = new User("12345", "Nat", null);
            roleService.AddRole(role);
            roleService.AddUser(user);
            var correctUser = roleService.GetUser(user.Id);

            var result = roleService.AssignRole(user.Id, role);
            // Check if the role was assigned, add the correctuser to the usermessage
            Assert.True(result, $"Valid roles should be assigned successfully. correctUser : {correctUser}");
        }

        [Fact]
        public void AssignRole_InvalidRole_ShouldFail()
        {
            var role = new Role("NonExistent", null);
            var user = new User("12345", "Nat", null);
            roleService.AddUser(user);
            var result = roleService.AssignRole(user.Id, role);
            Assert.False(result, "Role assignment should fail for invalid roles.");
        }

        [Fact]
        public void AssignRole_EmptyUserId_ShouldThrowException()
        {
            var role = new Role("Student", new List<string> { "Read" });
            var exception = Assert.Throws<ArgumentException>(() => roleService.AssignRole("", role));
            Assert.Equal("User ID cannot be empty.", exception.Message);
        }

        [Fact]
        public void AssignRole_InvalidUserId_ShouldThrowException()
        {
            var role = new Role("Student", new List<string> { "Read" });
            // User does not exist returns false
            var result = roleService
                .AssignRole("invalidId", role);
            Assert.False(result, "Role assignment should fail for invalid user IDs.");
        }

        // Access Control Tests
        [Fact]
        public void HasAccess_AdminRole_ShouldHaveAllPermissions()
        {
            var role = new Role("Admin", new List<string> { "ModifyRoles", "ParkingAccess" });
            var user = new User("admin123", "John", role);
            roleService.AddRole(role);
            roleService.AddUser(user);
            foreach (var permission in role.Permissions)
            {
                Assert.True(roleService.HasAccess(user.Id, permission), $"Admins should have access to {permission}.");
            }
        }

        [Fact]
        public void HasAccess_StaffRole_ShouldHaveLimitedPermissions()
        {
            var role = new Role("Staff", new List<string> { "ParkingAccess" });
            var user = new User("staff123", "Jane", role);
            roleService.AddRole(role);
            roleService.AddUser(user);
            Assert.True(roleService.HasAccess(user.Id, "ParkingAccess"), "Staff should have access to ParkingAccess.");
            Assert.False(roleService.HasAccess(user.Id, "ModifyRoles"), "Staff should not have access to ModifyRoles.");
        }

        [Fact]
        public void HasAccess_StudentRole_ShouldHaveLimitedPermissions()
        {
            var role = new Role("Student", new List<string> { "LibraryAccess" });
            var user = new User("student123", "Jojo", role);
            roleService.AddRole(role);
            roleService.AddUser(user);
            Assert.True(roleService.HasAccess(user.Id, "LibraryAccess"), "Students should have access to LibraryAccess.");
            Assert.False(roleService.HasAccess(user.Id, "ParkingAccess"), "Students should not have access to ParkingAccess.");
        }

        // Role Update Tests
        [Fact]
        public void UpdateUserRole_AdminToStaff_ShouldSucceed()
        {
            var adminRole = new Role("Admin", new List<string> { "Create", "Read", "Update", "Delete" });
            var staffRole = new Role("Staff", new List<string> { "Read" });
            var adminUser = new User("admin123", "AdminUser", adminRole);
            var adminUser2 = new User("admin456", "AdminUser2", adminRole);
            roleService.AddRole(adminRole);
            roleService.AddRole(staffRole);
            roleService.AddUser(adminUser);
            roleService.AddUser(adminUser2);
            var result = roleService.UpdateUserRole(adminUser.Id, adminUser.Id, staffRole);
            Assert.True(result, "Admin should be able to update their role to Staff.");
        }

        [Fact]
        public void UpdateUserRole_LastAdmin_ShouldThrowException()
        {
            var adminRole = new Role("Admin", null);
            var otherRole = new Role("Staff", null);
            roleService.AddRole(adminRole);
            roleService.AddRole(otherRole);
            roleService.AddUser(new User("admin123", "John", adminRole));
            var exception = Assert.Throws<InvalidOperationException>(() =>
                roleService.UpdateUserRole("admin123", "admin123", otherRole)
            );
            Assert.Equal("Cannot update the last remaining admin to another role.", exception.Message);
        }

        [Fact]
        public void UpdateUserRole_NonAdmin_ShouldThrowException()
        {
            var staffRole = new Role("Staff", null);
            var nonAdminUser = new User("staff123", "Jane", staffRole);
            roleService.AddRole(staffRole);
            roleService.AddUser(nonAdminUser);
            var exception = Assert.Throws<UnauthorizedAccessException>(() =>
                roleService.UpdateUserRole(nonAdminUser.Id, "user456", new Role("Student", null)));
            Assert.Equal("Only admins can update roles.", exception.Message);
        }

        [Fact]
        public void UpdateUserRole_InvalidRole_ShouldFail()
        {
            var adminRole = new Role("Admin", null);
            var adminUser = new User("admin123", "AdminUser", adminRole);
            roleService.AddRole(adminRole);
            roleService.AddUser(adminUser);
            var result = roleService.UpdateUserRole(adminUser.Id, adminUser.Id, new Role("InvalidRole", null));
            Assert.False(result, "Role update should fail if the new role is invalid.");
        }
    }
}
