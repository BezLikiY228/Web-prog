using UniversityApi.Models;

namespace UniversityApi.Repositories.Interfaces;

public interface ICourseRepository
{
    Task<List<Course>> GetAllAsync(
        int? teacherId = null,
        int? minCredits = null,
        int? maxCredits = null);

    Task<Course?> GetByIdAsync(int id);
    Task<Course> AddAsync(Course course);
    Task UpdateAsync(Course course);
    Task DeleteAsync(Course course);
    Task<bool> HasEnrollmentsAsync(int courseId);
}