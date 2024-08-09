namespace Iot.Database.Users;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public List<Permission> Permissions { get; set; } = new();
}
