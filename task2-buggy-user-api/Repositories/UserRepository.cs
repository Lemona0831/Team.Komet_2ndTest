using BuggyUserApi.Models;

namespace BuggyUserApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new();
    private int _nextId = 1;

    public User Add(User user)
    {
        user.Id = _nextId++;
        _users.Add(user);
        return user;
    }

    public List<User> GetAll()
    {
        return _users
            .Where(user => !user.IsDeleted)
            .ToList();
    }

    public User GetById(int id)
    {
        return _users.FirstOrDefault(user => user.Id == id && !user.IsDeleted);
    }

    public User GetByEmail(string email)
    {
        return _users.FirstOrDefault(user => !user.IsDeleted &&
                    string.Equals(user.Email,email,StringComparison.OrdinalIgnoreCase));
    }

    public bool Delete(int id)
    {
        var user = GetById(id);

        if(user == null || user.IsDeleted)
        {
            return false;
        }

        user.IsDeleted = true;
        return true;
    }
}
