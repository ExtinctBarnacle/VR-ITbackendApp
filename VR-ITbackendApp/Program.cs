using VR_ITbackendApp;
using VR_ITbackendApp.Controllers;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

DBService.CreateChatTable();
ToDoController toDoController = new ToDoController();
app.MapGet("/", () => "<a href=\"https://localhost:7102/api/todo\">Add Todo Item</a>");

app.MapGet("/api/todo", () => toDoController.Index(new VR_ITbackendApp.TodoItem())); // POST
//������� ����� ������� todo.
//��������� ���� ������� � ������� ��������������� ���� ��������� (HttpCode, 200/400/500�).

//app.MapGet("/api/todo", () => toDoController.ActionMethod()); // GET
//�������� ��� �������� todo.

app.MapGet("/api/todo/{id}", () => toDoController.ActionMethod()); // GET
//�������� ���� ������� todo �� ��� ��������������.
//������� 404 Not Found, ���� ������� �� ����������.

app.MapGet("/api/todo/{id}", () => toDoController.ActionMethod()); // PUT
//�������� ������������ ������� todo.
//������� 404 Not Found, ���� ������� �� ����������.

app.MapGet("/api/todo/{id}", () => toDoController.ActionMethod()); // DELETE
//������� ������������ ������� todo.
//������� 404 Not Found, ���� ������� �� ����������.

app.Run();
