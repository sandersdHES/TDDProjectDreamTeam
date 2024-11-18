using System;

namespace SchoolApp.RoleManagement.Models;

public class RoleAssignmentRequest
{
    public int RequestId { get; set; }
    public string AdminId { get; set; }
    public string UserId { get; set; }
    public string NewRole { get; set; }

    public RoleAssignmentRequest(int requestId, string adminId, string userId, string newRole)
    {
        RequestId = requestId;
        AdminId = adminId;
        UserId = userId;
        NewRole = newRole;
    }
}
