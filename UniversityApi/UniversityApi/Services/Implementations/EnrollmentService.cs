
using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityApi.DTO.Enrollments;
using UniversityApi.Models;
using UniversityApi.Repositories.Interfaces;
using UniversityApi.Services.Interfaces;

namespace UniversityApi.Services.Implementations;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<EnrollmentService> _logger;

    public EnrollmentService(
        IEnrollmentRepository enrollmentRepository,
        IStudentRepository studentRepository,
        ICourseRepository courseRepository,
        IMapper mapper,
        ILogger<EnrollmentService> logger)
    {
        _enrollmentRepository = enrollmentRepository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<EnrollmentDto>> GetAllAsync()
    {
        var enrollments = await _enrollmentRepository.GetAllAsync();

        return _mapper.Map<List<EnrollmentDto>>(enrollments);
    }

    public async Task<EnrollmentDto?> GetByIdAsync(int id)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(id);

        if (enrollment == null)
            return null;

        return _mapper.Map<EnrollmentDto>(enrollment);
    }

    public async Task<EnrollmentDto> CreateAsync(EnrollmentCreateDto dto)
    {
        var student = await _studentRepository.GetByIdAsync(dto.StudentId);

        if (student == null)
            throw new InvalidOperationException(
                "Нельзя записать студента: студент не найден.");

        var course = await _courseRepository.GetByIdAsync(dto.CourseId);

        if (course == null)
            throw new InvalidOperationException(
                "Нельзя записать студента: курс не найден.");

        var existingEnrollment =
            await _enrollmentRepository.GetByStudentAndCourseAsync(
                dto.StudentId, dto.CourseId);

        if (existingEnrollment != null)
            throw new InvalidOperationException(
                "Студент уже записан на этот курс.");

        var enrollment = _mapper.Map<Enrollment>(dto);
        enrollment.EnrollmentDate = DateTime.UtcNow;

        await _enrollmentRepository.AddAsync(enrollment);

        _logger.LogInformation(
            "Создана запись с Id {EnrollmentId}", enrollment.Id);

        var createdEnrollment =
            await _enrollmentRepository.GetByIdAsync(enrollment.Id);

        return _mapper.Map<EnrollmentDto>(createdEnrollment!);
    }

    public async Task<bool> UpdateAsync(int id, EnrollmentUpdateDto dto)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(id);

        if (enrollment == null)
            return false;

        enrollment.Grade = dto.Grade;

        await _enrollmentRepository.UpdateAsync(enrollment);

        _logger.LogInformation(
            "Обновлена оценка записи с Id {EnrollmentId}", id);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(id);

        if (enrollment == null)
            return false;

        await _enrollmentRepository.DeleteAsync(enrollment);

        _logger.LogInformation(
            "Удалена запись с Id {EnrollmentId}", id);

        return true;
    }
}