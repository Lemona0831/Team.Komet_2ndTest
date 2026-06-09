using BuggyUserApi.DTOs;
using BuggyUserApi.Repositories;
using BuggyUserApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<UserService>();

var app = builder.Build();

app.MapPost("/users", (UserCreateRequest request, UserService service) =>
{
    try
    {
        var user = service.CreateUser(request);
        return Results.Created($"/users/{user.Id}", user);
    }
    catch(ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(ex.Message);
    }
});

app.MapGet("/users", (UserService service) =>
{
    var users = service.GetUsers(); 
    return Results.Ok(users);
});

app.MapGet("/users/{id}", (int id, UserService service) =>
{
    var user = service.GetUser(id);

    if(user == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(user);
});

app.MapPatch("/users/{id}/role", (int id, RoleChangeRequest request, UserService service) =>
{
    try
    {
        var user = service.ChangeRole(id, request);
        if (user == null)
        {
            Results.NotFound();
        }

        return Results.Ok(user);
    }
    catch(Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/users/{id}", (int id, UserService service) =>
{
    var deleted = service.DeleteUser(id);

    if (!deleted)
    {
        return Results.NotFound();
    }
   

    return Results.NoContent();
});

app.Run();
