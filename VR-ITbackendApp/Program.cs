using VR_ITbackendApp;
using VR_ITbackendApp.Controllers;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

DBService.CreateToDoTable();
ToDoController toDoController = new ToDoController();
app.MapGet("/", () => toDoController.Index());

//������� ����� ������� todo
app.MapPost("/api/todo", () => {
    
    toDoController.AddToDo(new TodoItem("empty bin"));
    });

//�������� ��� �������� todo
app.MapGet("/api/todo", () => toDoController.GetAllToDos());

//�������� ���� ������� todo �� ��� ��������������
app.MapGet("/api/todo/{id}", () => toDoController.GetToDoById(3));

//�������� ������������ ������� todo
app.MapPut("/api/todo/{id}", () => toDoController.UpdateToDoById(new TodoItem("do smth")));


//������� ������������ ������� todo
app.MapDelete("/api/todo/{id}", () => toDoController.RemoveToDoById(1));


app.Run();
