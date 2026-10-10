using Microsoft.AspNetCore.Mvc;
using homework_3.Models;
using homework_3.Services;

namespace homework_3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(
            IStudentService studentService,
            ILogger<StudentsController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetAll()
        {
            _logger.LogInformation("Request to get all students");

            try
            {
                var students = _studentService.GetAll();

                return Ok(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while getting all students");

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Student> GetById(int id)
        {
            _logger.LogInformation(
                "Request to get student with ID {StudentId}",
                id);

            try
            {
                var student = _studentService.GetById(id);

                if (student == null)
                {
                    _logger.LogWarning(
                        "Student with ID {StudentId} was not found",
                        id);

                    return NotFound();
                }

                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while getting student with ID {StudentId}",
                    id);

                return StatusCode(500, "Internal server error");
            }
        }
    }
}