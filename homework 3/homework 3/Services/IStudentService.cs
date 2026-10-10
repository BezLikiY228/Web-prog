using homework_3.Models;

namespace homework_3.Services
{
    public interface IStudentService
    {
        IEnumerable<Student> GetAll();

        Student? GetById(int id);
    }
}