using Microsoft.AspNetCore.Mvc;
using WebLab1Ds.Models;

namespace WebLab1Ds.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private static List<Course> courses = new List<Course>
        {
            new Course
            {
                Id = 1,
                Name = "Web Services",
                Teacher = "Teacher 1",
                Credits = 5
            },

            new Course
            {
                Id = 2,
                Name = "Databases",
                Teacher = "Teacher 2",
                Credits = 4
            },

            new Course
            {
                Id = 3,
                Name = "Programming",
                Teacher = "Teacher 3",
                Credits = 6
            }
        };


        [HttpGet]
        public ActionResult<List<Course>> GetAll()
        {
            return Ok(courses);
        }


        [HttpGet("{id}")]
        public ActionResult<Course> GetById(int id)
        {
            var course = courses.FirstOrDefault(x => x.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }


        [HttpPost]
        public ActionResult<Course> Create(Course course)
        {
            course.Id = courses.Count > 0
                ? courses.Max(x => x.Id) + 1
                : 1;

            courses.Add(course);

            return CreatedAtAction(
                nameof(GetById),
                new { id = course.Id },
                course);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, Course updatedCourse)
        {
            var course = courses.FirstOrDefault(x => x.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            course.Name = updatedCourse.Name;
            course.Teacher = updatedCourse.Teacher;
            course.Credits = updatedCourse.Credits;

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var course = courses.FirstOrDefault(x => x.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            courses.Remove(course);

            return NoContent();
        }
    }
}