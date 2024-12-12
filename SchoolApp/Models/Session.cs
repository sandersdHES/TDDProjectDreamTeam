using System;

namespace SchoolApp.Models;

public class Session
{
    public string UserId { get; set; }
    public string CurrentRole { get; set; }

    public Session(string userId, string currentRole)
    {
        UserId = userId;
        CurrentRole = currentRole;
    }

    public void UpdateRole(string newRole)
    {
        CurrentRole = newRole;
    }
}

