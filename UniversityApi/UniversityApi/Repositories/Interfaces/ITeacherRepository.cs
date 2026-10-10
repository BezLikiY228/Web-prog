using UniversityApi.Models;

namespace UniversityApi.Repositories.Interfaces;

public interface ITeacherRepository
{
    Task<List<Teacher>> GetAllAsync();
    Task<Teacher?> GetByIdAsync(int id);
    Task<Teacher> AddAsync(Teacher teacher);
    Task UpdateAsync(Teacher teacher);
    Task DeleteAsync(Teacher teacher);
    Task<bool> EmailExistsAsync(string email, int? excludeId = null);
}