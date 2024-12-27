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

TodoService.CreateToDoTable();
Console.WriteLine("Приложение запущено. Целостность базы данных проверена.");
ToDoController toDoController = new ToDoController();

// регистрация пользователя - формирование токена
app.Map("/login/{username}", (string username, HttpContext context) =>
{
    ToDoMiddleware.RegisterRequest(context.Request);
    var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
    // создаем JWT-токен
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

//Создать новый элемент todo
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

//Получить все элементы todo
app.MapGet("/api/todo", 
    [Authorize]
[HttpGet] (HttpContext context) => {
    ToDoMiddleware.RegisterRequest(context.Request);
    return toDoController.GetAllToDos();
});

//Получить один элемент todo по его идентификатору
app.MapGet("/api/todo/{id}", 
    [Authorize]
    [HttpGet] (string id, HttpContext context) => {
    ToDoMiddleware.RegisterRequest(context.Request);
    return toDoController.GetToDoById(id);
});

//Обновить элемент todo по его идентификатору
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

//Удалить элемент todo по его идентификатору
app.MapDelete("/api/todo/{id}", 
    [Authorize]
[HttpDelete] (string id, HttpContext context) => {
    ToDoMiddleware.RegisterRequest(context.Request);
    return toDoController.RemoveToDoById(id)
;});

app.Run();