
using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityApi.DTO.Students;
using UniversityApi.Repositories.Interfaces;
using UniversityApi.Services.Interfaces;
using UniversityApi.Models;

namespace UniversityApi.Services.Implementations;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<StudentService> _logger;

    public StudentService(
        IStudentRepository repository,
        IMapper mapper,
        ILogger<StudentService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<StudentDto>> GetAllAsync()
    {
        var students = await _repository.GetAllAsync();
        return _mapper.Map<List<StudentDto>>(students);
    }

    public async Task<StudentDto?> GetByIdAsync(int id)
    {
        var student = await _repository.GetByIdAsync(id);

        if (student == null)
            return null;

        return _mapper.Map<StudentDto>(student);
    }

    public async Task<StudentDto> CreateAsync(StudentCreateDto dto)
    {
        if (await _repository.EmailExistsAsync(dto.Email))
            throw new InvalidOperationException(
                "Студент с таким Email уже существует.");

        var student = _mapper.Map<Student>(dto);
        student.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(student);

        _logger.LogInformation(
            "Создан студент с Id {StudentId}", student.Id);

        return _mapper.Map<StudentDto>(student);
    }

    public async Task<bool> UpdateAsync(int id, StudentUpdateDto dto)
    {
        var student = await _repository.GetByIdAsync(id);

        if (student == null)
            return false;

        if (await _repository.EmailExistsAsync(dto.Email, id))
            throw new InvalidOperationException(
                "Другой студент уже использует этот Email.");

        _mapper.Map(dto, student);

        await _repository.UpdateAsync(student);

        _logger.LogInformation(
            "Обновлён студент с Id {StudentId}", id);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var student = await _repository.GetByIdAsync(id);

        if (student == null)
            return false;

        await _repository.DeleteAsync(student);

        _logger.LogInformation(
            "Удалён студент с Id {StudentId}", id);

        return true;
    }
}