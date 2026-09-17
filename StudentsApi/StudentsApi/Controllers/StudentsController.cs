using Microsoft.AspNetCore.Mvc;
using StudentsApi.Models;

namespace StudentsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private static List<Student> students = new()
        {
            new Student
            {
                Id = 1,
                Name = "Alex",
                Group = "SE-301"
            },

            new Student
            {
                Id = 2,
                Name = "Anna",
                Group = "SE-302"
            },

            new Student
            {
                Id = 3,
                Name = "Max",
                Group = "SE-301"
            }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            return Ok(students);
        }

        [HttpGet("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpPost]
        public ActionResult<Student> CreateStudent(Student student)
        {
            students.Add(student);

            return Ok(student);
        }
    }
}