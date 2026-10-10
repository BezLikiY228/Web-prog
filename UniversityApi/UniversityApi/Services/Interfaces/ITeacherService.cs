
using UniversityApi.DTO.Teachers;

namespace UniversityApi.Services.Interfaces;

public interface ITeacherService
{
    Task<List<TeacherDto>> GetAllAsync();
    Task<TeacherDto?> GetByIdAsync(int id);
    Task<TeacherDto> CreateAsync(TeacherCreateDto dto);
    Task<bool> UpdateAsync(int id, TeacherUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}