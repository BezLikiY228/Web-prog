
using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityApi.DTO.Teachers;
using UniversityApi.Models;
using UniversityApi.Repositories.Interfaces;
using UniversityApi.Services.Interfaces;

namespace UniversityApi.Services.Implementations;

public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<TeacherService> _logger;

    public TeacherService(
        ITeacherRepository repository,
        IMapper mapper,
        ILogger<TeacherService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<TeacherDto>> GetAllAsync()
    {
        var teachers = await _repository.GetAllAsync();
        return _mapper.Map<List<TeacherDto>>(teachers);
    }

    public async Task<TeacherDto?> GetByIdAsync(int id)
    {
        var teacher = await _repository.GetByIdAsync(id);

        if (teacher == null)
            return null;

        return _mapper.Map<TeacherDto>(teacher);
    }

    public async Task<TeacherDto> CreateAsync(TeacherCreateDto dto)
    {
        if (await _repository.EmailExistsAsync(dto.Email))
            throw new InvalidOperationException(
                "Преподаватель с таким Email уже существует.");

        var teacher = _mapper.Map<Teacher>(dto);

        await _repository.AddAsync(teacher);

        _logger.LogInformation(
            "Создан преподаватель с Id {TeacherId}", teacher.Id);

        return _mapper.Map<TeacherDto>(teacher);
    }

    public async Task<bool> UpdateAsync(int id, TeacherUpdateDto dto)
    {
        var teacher = await _repository.GetByIdAsync(id);

        if (teacher == null)
            return false;

        if (await _repository.EmailExistsAsync(dto.Email, id))
            throw new InvalidOperationException(
                "Другой преподаватель уже использует этот Email.");

        _mapper.Map(dto, teacher);

        await _repository.UpdateAsync(teacher);

        _logger.LogInformation(
            "Обновлён преподаватель с Id {TeacherId}", id);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var teacher = await _repository.GetByIdAsync(id);

        if (teacher == null)
            return false;

        await _repository.DeleteAsync(teacher);

        _logger.LogInformation(
            "Удалён преподаватель с Id {TeacherId}", id);

        return true;
    }
}