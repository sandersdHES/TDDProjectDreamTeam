using System;

namespace SchoolApp.Models;

public class Role
{
    public string Name { get; set; }
    public List<string> Permissions { get; set; }

    public Role(string name, List<string> permissions)
    {
        Name = name;
        Permissions = permissions ?? new List<string>();
    }

    public bool HasPermission(string feature)
    {
        return Permissions.Contains(feature, StringComparer.OrdinalIgnoreCase);
    }

    public void AddPermission(string feature)
    {
        Permissions.Add(feature);
    }

    public void RemovePermission(string feature)
    {
        Permissions.Remove(feature);
    }
}
