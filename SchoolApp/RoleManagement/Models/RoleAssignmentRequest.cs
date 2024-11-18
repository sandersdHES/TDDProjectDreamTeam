using System;

namespace SchoolApp.RoleManagement.Models;

public class RoleAssignmentRequest
{
    public string AdminId { get; set; }
    public string UserId { get; set; }
    public string NewRole { get; set; }

    public RoleAssignmentRequest(string adminId, string userId, string newRole)
    {
        AdminId = adminId;
        UserId = userId;
        NewRole = newRole;
    }
}
