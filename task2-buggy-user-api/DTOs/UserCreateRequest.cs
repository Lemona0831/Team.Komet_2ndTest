namespace BuggyUserApi.DTOs;

public class UserCreateRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
}
