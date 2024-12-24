using Microsoft.AspNetCore.Mvc;

namespace VR_ITbackendApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : Controller
    {
        [HttpPost]
        public IActionResult Index([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest("Invalid request");
            }
            List<TodoItem> todo = new List<TodoItem>();
            item.Id = 1;
            item.Title = "empty bin";
            item.IsCompleted = true;
            item.CreatedAt = DateTime.Now;
            todo.Add(item);
            DBService.StoreDataToDB(item);
            var response = new
            {
                Message = "Request processed successfully!",
                ReceivedData = item
            };

            return Ok(response);
        }
        public IActionResult ActionMethod()
        {
            return View();
        }
    }
}