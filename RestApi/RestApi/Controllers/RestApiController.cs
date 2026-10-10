using Microsoft.AspNetCore.Mvc;
using RestApi.Models;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class RestApiController : ControllerBase
    {
        private static readonly List<TaskItem> tasks = new List<TaskItem>
        {
            new TaskItem
            {
                Id = 1,
                Title = "Изучить ASP.NET Core",
                Description = "Изучить создание Web API",
                IsCompleted = false
            },
            new TaskItem
            {
                Id = 2,
                Title = "Создать Swagger API",
                Description = "Протестировать методы через Swagger",
                IsCompleted = true
            }
        };

      
        [HttpGet]
        public ActionResult<IEnumerable<TaskItem>> GetTasks()
        {
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public ActionResult<TaskItem> GetTask(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPost]
        public ActionResult<TaskItem> CreateTask(TaskItem task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                return BadRequest("Название задачи не может быть пустым.");
            }

            task.Id = tasks.Count == 0 ? 1 : tasks.Max(t => t.Id) + 1;

            tasks.Add(task);

            return CreatedAtAction(
                nameof(GetTask),
                new { id = task.Id },
                task
            );
        }

        [HttpPut("{id}")]
        public ActionResult UpdateTask(int id, TaskItem updatedTask)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(updatedTask.Title))
            {
                return BadRequest("Название задачи не может быть пустым.");
            }

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.IsCompleted = updatedTask.IsCompleted;

            return Ok(task);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteTask(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            tasks.Remove(task);

            return Ok(task);
        }
    }
}