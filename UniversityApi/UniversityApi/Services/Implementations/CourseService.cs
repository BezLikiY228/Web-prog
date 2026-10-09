
using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityApi.DTO.Courses;
using UniversityApi.Models;
using UniversityApi.Repositories.Interfaces;
using UniversityApi.Services.Interfaces;

namespace UniversityApi.Services.Implementations;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CourseService> _logger;

    public CourseService(
        ICourseRepository courseRepository,
        ITeacherRepository teacherRepository,
        IMapper mapper,
        ILogger<CourseService> logger)
    {
        _courseRepository = courseRepository;
        _teacherRepository = teacherRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<CourseDto>> GetAllAsync(
        int? teacherId = null,
        int? minCredits = null,
        int? maxCredits = null)
    {
        var courses = await _courseRepository.GetAllAsync(
            teacherId, minCredits, maxCredits);

        return _mapper.Map<List<CourseDto>>(courses);
    }

    public async Task<CourseDto?> GetByIdAsync(int id)
    {
        var course = await _courseRepository.GetByIdAsync(id);

        if (course == null)
            return null;

        return _mapper.Map<CourseDto>(course);
    }

    public async Task<CourseDto> CreateAsync(CourseCreateDto dto)
    {
        var teacher = await _teacherRepository.GetByIdAsync(dto.TeacherId);

        if (teacher == null)
            throw new InvalidOperationException(
                "Нельзя создать курс: преподаватель не найден.");

        var course = _mapper.Map<Course>(dto);
        course.CreatedAt = DateTime.UtcNow;

        await _courseRepository.AddAsync(course);

        _logger.LogInformation(
            "Создан курс с Id {CourseId}", course.Id);

        
        var createdCourse = await _courseRepository.GetByIdAsync(course.Id);

        return _mapper.Map<CourseDto>(createdCourse!);
    }

    public async Task<bool> UpdateAsync(int id, CourseUpdateDto dto)
    {
        var course = await _courseRepository.GetByIdAsync(id);

        if (course == null)
            return false;

        var teacher = await _teacherRepository.GetByIdAsync(dto.TeacherId);

        if (teacher == null)
            throw new InvalidOperationException(
                "Нельзя изменить курс: преподаватель не найден.");

        _mapper.Map(dto, course);

        await _courseRepository.UpdateAsync(course);

        _logger.LogInformation(
            "Обновлён курс с Id {CourseId}", id);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var course = await _courseRepository.GetByIdAsync(id);

        if (course == null)
            return false;

        if (await _courseRepository.HasEnrollmentsAsync(id))
            throw new InvalidOperationException(
                "Нельзя удалить курс, на который уже записаны студенты.");

        await _courseRepository.DeleteAsync(course);

        _logger.LogInformation(
            "Удалён курс с Id {CourseId}", id);

        return true;
    }
}