using homework_3.Models;

namespace homework_3.Services
{
    public class StudentService : IStudentService
    {
        private readonly List<Student> _students = new List<Student>
        {
            new Student
            {
                Id = 1,
                Name = "Islam",
                Group = "CSE-23-01"
            },

            new Student
            {
                Id = 2,
                Name = "Ali",
                Group = "CSE-23-02"
            },

            new Student
            {
                Id = 3,
                Name = "Daniyar",
                Group = "CSE-23-01"
            }
        };

        public IEnumerable<Student> GetAll()
        {
            return _students;
        }

        public Student? GetById(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id);
        }
    }
}