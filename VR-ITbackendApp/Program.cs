using VR_ITbackendApp;
using VR_ITbackendApp.Controllers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VR_ITbackendApp.Models;
using VR_ITbackendApp.Middleware;

var builder = WebApplication.CreateBuilder();

ToDoMiddleware.GetAuthToken(builder);

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();

// creates new TODO table if it does not exist
TodoService.CreateToDoTable();
Console.WriteLine("App has started. Database availability was checked."); // ���������� ��������. ����������� ���� ������ ���������.
ToDoController toDoController = new ToDoController();

// ����������� ������������ - ������������ ������
//creates token for user authorization
app.Map("/login/{username}", (string username, HttpContext context) =>
{
    ToDoMiddleware.RegisterRequest(context.Request);
    var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
    // �������� JWT-������
    var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(200)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
            SecurityAlgorithms.HmacSha256));
    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
    var response = new
    {
        access_token = encodedJwt,
        username = username
    };
    return response;

});

//������� ����� ������� todo
//adds todo in DB
app.MapPost("/api/todo", 
    [Authorize]
[HttpPost] (HttpContext context) => {
    ToDoMiddleware.RegisterRequest(context.Request);
    //if (context.Request.Method == "POST") ;
    TodoItem toDo = new TodoItem("");
    foreach (var param in context.Request.Query)
    {
        if (param.Key == "Title") toDo.Title = param.Value;
        if (param.Key == "isCompleted") toDo.IsCompleted = param.Value == "0" ? false : true;
    }
    toDo.CreatedAt = DateTime.Now;
    return toDoController.AddToDo(toDo);
    });

//�������� ��� �������� todo
//gets all todo's from DB
app.MapGet("/api/todo", 
    [Authorize]
[HttpGet] (HttpContext context) => {
    ToDoMiddleware.RegisterRequest(context.Request);
    return toDoController.GetAllToDos();
});

//�������� ���� ������� todo �� ��� ��������������
//gets todo by index
app.MapGet("/api/todo/{id}", 
    [Authorize]
    [HttpGet] (string id, HttpContext context) => {
    ToDoMiddleware.RegisterRequest(context.Request);
    return toDoController.GetToDoById(id);
});

//�������� ������� todo �� ��� ��������������
//updates todo by index
app.MapPut("/api/todo/{id}", 
    [Authorize]
    [HttpPut] (string id, HttpContext context) => {
    ToDoMiddleware.RegisterRequest(context.Request);
    TodoItem toDo = new TodoItem("");
    foreach (var param in context.Request.Query)
    {
         if (param.Key == "Title") toDo.Title = param.Value;
         if (param.Key == "isCompleted") toDo.IsCompleted = param.Value == "0" ? false : true;
    }
    return toDoController.UpdateToDoById(id, toDo);
});

//������� ������� todo �� ��� ��������������
//removes todo by index
app.MapDelete("/api/todo/{id}", 
    [Authorize]
[HttpDelete] (string id, HttpContext context) => {
    ToDoMiddleware.RegisterRequest(context.Request);
    return toDoController.RemoveToDoById(id)
;});

app.Run();