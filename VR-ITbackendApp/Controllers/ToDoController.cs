using Microsoft.AspNetCore.Mvc;
using VR_ITbackendApp.Util;

namespace VR_ITbackendApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : Controller
    {
        public IActionResult Index()
        {
        var response = new 
        {
            ContentType = "text/html; charset=utf-8",
            ReceivedData = "<html><body><a href=\"https://localhost:7102/api/todo\">Add Todo Item</a></body></html>"
        };
            return Ok(response);
        }
        
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
                Message = "Todo successfully added:",
                ReceivedData = item
            };
            return Ok(response);
        }
        
        public IActionResult GetAllToDos()
        {
            TodoItem[] todo = TodoService.LoadToDoTable();
            var response = new
            {
                Message = "Database contains todos:",
                ReceivedData = todo
            };
            return Ok(response);
        }
        
        public IActionResult GetToDoById(string Id)
        {
            if (!int.TryParse(Id, out int result)) return NotFound();
            int id = int.Parse(Id);
            TodoItem todo = TodoService.LoadToDoById(id);
            if (todo == null) return NotFound();
            todo.Id = id;
            var response = new
            {
                Message = $"Todo with index {Id} successfully found",
                ReceivedData = todo
            };
            return Ok(response);
        }
       
        public IActionResult UpdateToDoById(string Id, TodoItem item)
        {
            if (!int.TryParse(Id, out int result)) return NotFound();
            int id = int.Parse(Id);
            item.Id = id;
            bool outcome = TodoService.UpdateDataToDB(item);
            if (!outcome) return NotFound();
            var response = new
            {
                Message = $"Todo with index {item.Id} successfully updated",
                ReceivedData = item
            };
            return Ok(response);
        }
        
        public IActionResult RemoveToDoById(string Id)
        {
            if (!int.TryParse(Id, out int result)) return NotFound();
            int id = int.Parse(Id);
            bool removed = TodoService.RemoveToDoById(id);
            if (!removed) return NotFound();
            var response = new
            {
                Message = $"Todo with index {Id} successfully removed",
                ReceivedData = ""
            };
            return Ok(response);
        }
    }
}