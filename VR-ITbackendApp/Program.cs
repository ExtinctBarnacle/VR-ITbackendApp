using VR_ITbackendApp;
using VR_ITbackendApp.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.Data;

var builder = WebApplication.CreateBuilder();
TodoService.CreateToDoTable();
ToDoController toDoController = new ToDoController();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
{
options.TokenValidationParameters = new TokenValidationParameters
{
// ���������, ����� �� �������������� �������� ��� ��������� ������
ValidateIssuer = true,
// ��������
ValidIssuer = AuthOptions.ISSUER,
// ����� �� �������������� ����������� ������
ValidateAudience = true,
// ��������� ����������� ������
ValidAudience = AuthOptions.AUDIENCE,
// ����� �� �������������� ����� �������������
ValidateLifetime = true,
// ��������� ����� ������������
IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
// ��������� ����� ������������
ValidateIssuerSigningKey = true,
};
});

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.Map("/login/{username}", (string username,HttpContext context) =>
{
    ToDoMiddleware.RegisterRequest(context.Request);
    var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
// ������� JWT-�����
var jwt = new JwtSecurityToken(
        issuer: AuthOptions.ISSUER,
        audience: AuthOptions.AUDIENCE,
        claims: claims,
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
return new JwtSecurityTokenHandler().WriteToken(jwt);
});

//app.Map("/data", [Authorize] (HttpContext context) => {
//    ToDoMiddleware.RegisterRequest(context.Request);
//    new { message = "Hello World!" };

//});

app.MapGet("/", [HttpGet] (HttpContext context) =>  {
    ToDoMiddleware.RegisterRequest(context.Request);
    toDoController.Index();
});

//������� ����� ������� todo
app.MapPost("/api/todo", [HttpPost] (context) => {
    ToDoMiddleware.RegisterRequest(context.Request);
    //if (context.Request.Method == "POST") ;
    TodoItem toDo = new TodoItem("");
    foreach (var param in context.Request.Query)
    {
        if (param.Key == "Title") toDo.Title = param.Value;
        if (param.Key == "IsCompleted") toDo.IsCompleted = bool.Parse(param.Value.ToString());
    }
    toDo.CreatedAt = DateTime.Now;
    toDoController.AddToDo(toDo);
    return Task.CompletedTask;
    });

//�������� ��� �������� todo
app.MapGet("/api/todo", [HttpGet] (context) => {
    ToDoMiddleware.RegisterRequest(context.Request);
    toDoController.GetAllToDos();
    return Task.CompletedTask;
});

//�������� ���� ������� todo �� ��� ��������������
app.MapGet("/api/todo/{id}", [HttpGet] (string id, HttpContext context) => {
    ToDoMiddleware.RegisterRequest(context.Request);
    toDoController.GetToDoById(id);
});

//�������� ������� todo �� ��� ��������������
app.MapPut("/api/todo/{id}", [HttpPut] (context) => {
    ToDoMiddleware.RegisterRequest(context.Request);
    toDoController.UpdateToDoById(new TodoItem("do smth"));
    return Task.CompletedTask;
});

//������� ������� todo �� ��� ��������������
app.MapDelete("/api/todo/{id}", [HttpDelete] (string id, HttpContext context) => {
    ToDoMiddleware.RegisterRequest(context.Request);
    toDoController.RemoveToDoById(id)
;});

app.Run();

public class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; // �������� ������
    public const string AUDIENCE = "MyAuthClient"; // ����������� ������
    const string KEY = "a_key_for_secure_authorization_is_needed_to_be_very_long!!!";   // ���� ��� ��������
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}
