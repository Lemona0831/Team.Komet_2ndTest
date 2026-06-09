using BuggyUserApi.Models;

namespace BuggyUserApi.Repositories;

public interface IUserRepository
{
    User Add(User user);
    List<User> GetAll();
    User GetById(int id);
    User GetByEmail(string email);
    bool Delete(int id);
}
