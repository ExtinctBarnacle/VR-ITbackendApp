using VR_ITbackendApp;
using VR_ITbackendApp.Controllers;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

DBService.CreateChatTable();
ToDoController toDoController = new ToDoController();
app.MapGet("/", () => "<a href=\"https://localhost:7102/api/todo\">Add Todo Item</a>");

app.MapGet("/api/todo", () => toDoController.Index(new VR_ITbackendApp.TodoItem())); // POST
//Создать новый элемент todo.
//Проверить тело запроса и вернуть соответствующие коды состояния (HttpCode, 200/400/500…).

//app.MapGet("/api/todo", () => toDoController.ActionMethod()); // GET
//Получить все элементы todo.

app.MapGet("/api/todo/{id}", () => toDoController.ActionMethod()); // GET
//Получить один элемент todo по его идентификатору.
//Вернуть 404 Not Found, если элемент не существует.

app.MapGet("/api/todo/{id}", () => toDoController.ActionMethod()); // PUT
//Обновить существующий элемент todo.
//Вернуть 404 Not Found, если элемент не существует.

app.MapGet("/api/todo/{id}", () => toDoController.ActionMethod()); // DELETE
//Удалить существующий элемент todo.
//Вернуть 404 Not Found, если элемент не существует.

app.Run();
