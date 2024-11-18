using System;

namespace SchoolApp.RoleManagement.Models;

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }

    public User(string id, string name, string role)
    {
        Id = id;
        Name = name;
        Role = role;
    }
}
