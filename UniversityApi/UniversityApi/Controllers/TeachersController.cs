
using Microsoft.AspNetCore.Mvc;
using UniversityApi.Common;
using UniversityApi.DTO.Teachers;
using UniversityApi.Services.Interfaces;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _service;

    public TeachersController(ITeacherService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ReturnResult<List<TeacherDto>>>> GetAll()
    {
        var teachers = await _service.GetAllAsync();

        var response = ReturnResult<List<TeacherDto>>.Success(teachers);
        response.TraceId = HttpContext.TraceIdentifier;

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReturnResult<TeacherDto>>> GetById(int id)
    {
        var teacher = await _service.GetByIdAsync(id);

        if (teacher == null)
        {
            return NotFound(ReturnResult<TeacherDto>.Failure(
                404,
                "TEACHER_NOT_FOUND",
                "Преподаватель не найден.",
                HttpContext.TraceIdentifier));
        }

        var response = ReturnResult<TeacherDto>.Success(teacher);
        response.TraceId = HttpContext.TraceIdentifier;

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ReturnResult<TeacherDto>>> Create(
        TeacherCreateDto dto)
    {
        try
        {
            var teacher = await _service.CreateAsync(dto);

            var response = ReturnResult<TeacherDto>.Success(teacher, 201);
            response.TraceId = HttpContext.TraceIdentifier;

            return StatusCode(201, response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ReturnResult<TeacherDto>.Failure(
                409,
                "TEACHER_CONFLICT",
                ex.Message,
                HttpContext.TraceIdentifier));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ReturnResult<TeacherDto>>> Update(
        int id,
        TeacherUpdateDto dto)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(ReturnResult<TeacherDto>.Failure(
                    404,
                    "TEACHER_NOT_FOUND",
                    "Преподаватель не найден.",
                    HttpContext.TraceIdentifier));
            }

            var teacher = await _service.GetByIdAsync(id);

            var response = ReturnResult<TeacherDto>.Success(teacher!);
            response.TraceId = HttpContext.TraceIdentifier;

            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ReturnResult<TeacherDto>.Failure(
                409,
                "TEACHER_CONFLICT",
                ex.Message,
                HttpContext.TraceIdentifier));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ReturnResult<object>>> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(ReturnResult<object>.Failure(
                404,
                "TEACHER_NOT_FOUND",
                "Преподаватель не найден.",
                HttpContext.TraceIdentifier));
        }

        var response = ReturnResult<object>.Success(new
        {
            Message = "Преподаватель успешно удалён."
        });

        response.TraceId = HttpContext.TraceIdentifier;

        return Ok(response);
    }
}