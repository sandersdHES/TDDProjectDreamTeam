using System;

namespace SchoolApp.Models;

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public Role Role { get; set; }

    public User(string id, string name, Role role)
    {
        Id = id;
        Name = name;
        Role = role;
    }

    public User(string id, string name, string email, string passwordHash, Role role)
    {
        Id = id;
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }
}
