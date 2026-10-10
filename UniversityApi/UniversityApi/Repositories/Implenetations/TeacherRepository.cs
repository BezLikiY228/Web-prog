using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Models;
using UniversityApi.Repositories.Interfaces;

namespace UniversityApi.Repositories.Implementations;

public class TeacherRepository : ITeacherRepository
{
    private readonly ApplicationDbContext _context;

    public TeacherRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Teacher>> GetAllAsync()
    {
        return await _context.Teachers
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Teacher?> GetByIdAsync(int id)
    {
        return await _context.Teachers
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Teacher> AddAsync(Teacher teacher)
    {
        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();
        return teacher;
    }

    public async Task UpdateAsync(Teacher teacher)
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Teacher teacher)
    {
        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> EmailExistsAsync(
        string email, int? excludeId = null)
    {
        return await _context.Teachers.AnyAsync(t =>
            t.Email == email &&
            (!excludeId.HasValue || t.Id != excludeId.Value));
    }
}