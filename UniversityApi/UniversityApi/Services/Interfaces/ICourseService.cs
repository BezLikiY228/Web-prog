
using UniversityApi.DTO.Courses;

namespace UniversityApi.Services.Interfaces;

public interface ICourseService
{
    Task<List<CourseDto>> GetAllAsync(
        int? teacherId = null,
        int? minCredits = null,
        int? maxCredits = null);

    Task<CourseDto?> GetByIdAsync(int id);
    Task<CourseDto> CreateAsync(CourseCreateDto dto);
    Task<bool> UpdateAsync(int id, CourseUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}