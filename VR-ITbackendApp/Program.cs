using VR_ITbackendApp;
using VR_ITbackendApp.Controllers;
var builder = WebApplication.CreateBuilder (args);
var app = builder.Build();

DBService.CreateToDoTable();
ToDoController toDoController = new ToDoController();
app.MapGet("/", () => toDoController.Index());

//������� ����� ������� todo
app.MapPost("/api/todo", (context) => {

    //if (context.Request.Method == "POST") ;
    TodoItem toDo = new TodoItem("");
    foreach (var param in context.Request.Query)
    {
        if(param.Key == "Title") toDo.Title = param.Value;
        if (param.Key == "IsCompleted") toDo.IsCompleted = bool.Parse(param.Value.ToString());
    }
    toDo.CreatedAt = DateTime.Now;
    toDoController.AddToDo(toDo);
    return Task.CompletedTask;
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
