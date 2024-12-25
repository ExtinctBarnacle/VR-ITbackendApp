using Microsoft.AspNetCore.Mvc;

namespace VR_ITbackendApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
        var response = new 
        {
            ReceivedData = "<html><body><a href=\"https://localhost:7102/api/todo\">Add Todo Item</a></body></html>",
            ContentType = "text/html; charset=utf-8"
        };
            return Ok(response);
        }

        [HttpPost]
        public IActionResult AddToDo ([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest("Invalid request");
            }
            if (item.Title == string.Empty)
            {
                return BadRequest("Invalid request");
            }
            TodoService.StoreDataToDB(item);
            var response = new
            {
                Message = "Request processed successfully!",
                ReceivedData = item
            };

            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetAllToDos()
        {
            TodoItem[] todo = TodoService.LoadToDoTable();
            var response = new
            {
                Message = "Request processed successfully!",
                ReceivedData = todo
            };

            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetToDoById(int Id)
        {
            TodoItem todo = TodoService.LoadToDoById(Id);
            if (todo == null) return NotFound();
            todo.Id = Id;
            var response = new
            {
                Message = "Request processed successfully!",
                ReceivedData = todo
            };

            return Ok(response);
        }
        [HttpPut]
        public IActionResult UpdateToDoById(TodoItem item)
        {
            bool result = TodoService.UpdateDataToDB(item);
            if (!result) return NotFound();
            var response = new
            {
                Message = "Request processed successfully!",
                ReceivedData = ""
            };

            return Ok(response);
        }
        [HttpDelete]
        public IActionResult RemoveToDoById(int Id)
        {
            bool result = TodoService.RemoveToDoById(Id);
            if (!result) return NotFound();
            var response = new
            {
                Message = "Request processed successfully!",
                ReceivedData = ""
            };

            return Ok(response);
        }
    }
}