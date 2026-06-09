using BuggyUserApi.DTOs;
using BuggyUserApi.Models;
using BuggyUserApi.Repositories;

namespace BuggyUserApi.Services;

public class UserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public UserResponse CreateUser(UserCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new ArgumentException("email은 비어 있을 수 없습니다.");
        }
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("password은 비어 있을 수 없습니다.");
        }
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("name은 비어 있을 수 없습니다.");
        }
        if (!isVaildRole(request.Role))
        {
            throw new ArgumentException("role은 USER 또는 ADMIN만 사용할 수 있습니다.");
        }
        var existingUser = _repository.GetByEmail(request.Email);
        if(existingUser != null)
        {
            throw new InvalidOperationException("이미 존재하는 email입니다.");
        }

        var user = new User
        {
            Email = request.Email,
            Password = request.Password,
            Name = request.Name,
            Role = request.Role,
            IsDeleted = false
        };

        var savedUser = _repository.Add(user);
        return ToResponse(savedUser);
    }

    public List<UserResponse> GetUsers()
    {
        return _repository.GetAll().Select(ToResponse).ToList();
    }

    public UserResponse GetUser(int id)
    {
        var user = _repository.GetById(id);

        if (user == null)
            return null;

        return ToResponse(user);
    }

    public UserResponse? ChangeRole(int id, RoleChangeRequest request)
    {
        if (!isVaildRole(request.Role))
        {
            throw new ArgumentException("role은 USER 또는 ADMIN만 사용할 수 있습니다.");
        }
        var user = _repository.GetById(id);

        if (user == null)
        {
            return null;
        }

        user.Role = request.Role;
        return ToResponse(user);
    }

    public bool DeleteUser(int id)
    {
        return _repository.Delete(id);
    }

    private UserResponse ToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Email,
            Role = user.Role
        };
    }

    private bool isVaildRole(string role)
    {
        return role == "USER" || role == "ADMIN";
    }
}
