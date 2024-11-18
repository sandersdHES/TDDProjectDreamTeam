using System;

namespace SchoolApp.RoleManagement.Models;

public class AccessRequest
{
    public string UserId { get; set; }
    public string Feature { get; set; }

    public AccessRequest(string userId, string feature)
    {
        UserId = userId;
        Feature = feature;
    }
}

