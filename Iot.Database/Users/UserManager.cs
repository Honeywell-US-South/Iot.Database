using Iot.Database.Table;
using System.Collections.Concurrent;
using System.Data;

namespace Iot.Database.Users;

internal class UserManager
{
   private readonly IotDatabase _database;
    private readonly ITableCollection<User> _usersTable;
    private readonly ConcurrentDictionary<string, User> _users = new ConcurrentDictionary<string, User>();
    public UserManager(IotDatabase iotDb)
    {
        _database = iotDb;
        _usersTable = new TableCollection<User>(_database, "Sys_Users");
    }

    public bool AddUser(string username, string password, Permission permission)
    {
        var user = _usersTable.FindOne(x=>x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        if (user != null)
        {
            throw new DuplicateNameException("Duplicate username found.");
        }
        user = new User
        {
            Username = username,
            PasswordHash = HashPassword(password),
            Permissions = new(new[] { permission })
        };
        var id = _usersTable.Insert(user);
        if (id.IsNull) return false;
        return true;
    }

    public bool Authenticate(string username, string password)
    {
        
        var user = _usersTable.FindOne(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        if (user == null)
        {
            if (_users.ContainsKey(username)) _users.Remove(username, out _);
            return false;
        }
       
        if (VerifyPassword(password, user.PasswordHash))
        {
            _users[user.Username] = user;
        } else
        {
            if (_users.ContainsKey(user.Username)) _users.Remove(user.Username, out _);
            return false;
        }
        return true;
    }

    

    private string HashPassword(string password)
    {
        // Implement a password hashing function here
        return password; // Placeholder
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        // Implement password verification
        return HashPassword(password) == hashedPassword; // Placeholder
    }
}
