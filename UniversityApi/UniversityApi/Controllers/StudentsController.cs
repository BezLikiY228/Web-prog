
using Microsoft.AspNetCore.Mvc;
using UniversityApi.Common;
using UniversityApi.DTO.Students;
using UniversityApi.Services.Interfaces;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _service;

    public StudentsController(IStudentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ReturnResult<List<StudentDto>>>> GetAll()
    {
        var students = await _service.GetAllAsync();

        var response = ReturnResult<List<StudentDto>>.Success(students);

        response.TraceId = HttpContext.TraceIdentifier;

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReturnResult<StudentDto>>> GetById(int id)
    {
        var student = await _service.GetByIdAsync(id);

        if (student == null)
        {
            return NotFound(ReturnResult<StudentDto>.Failure(
                404,
                "STUDENT_NOT_FOUND",
                "Студент не найден.",
                HttpContext.TraceIdentifier));
        }

        var response = ReturnResult<StudentDto>.Success(student);
        response.TraceId = HttpContext.TraceIdentifier;

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ReturnResult<StudentDto>>> Create(
        StudentCreateDto dto)
    {
        try
        {
            var student = await _service.CreateAsync(dto);

            var response = ReturnResult<StudentDto>.Success(student, 201);
            response.TraceId = HttpContext.TraceIdentifier;

            return StatusCode(201, response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ReturnResult<StudentDto>.Failure(
                409,
                "STUDENT_CONFLICT",
                ex.Message,
                HttpContext.TraceIdentifier));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ReturnResult<StudentDto>>> Update(
        int id,
        StudentUpdateDto dto)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(ReturnResult<StudentDto>.Failure(
                    404,
                    "STUDENT_NOT_FOUND",
                    "Студент не найден.",
                    HttpContext.TraceIdentifier));
            }

            var student = await _service.GetByIdAsync(id);

            var response = ReturnResult<StudentDto>.Success(student!);
            response.TraceId = HttpContext.TraceIdentifier;

            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ReturnResult<StudentDto>.Failure(
                409,
                "STUDENT_CONFLICT",
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
                "STUDENT_NOT_FOUND",
                "Студент не найден.",
                HttpContext.TraceIdentifier));
        }

        var response = ReturnResult<object>.Success(new
        {
            Message = "Студент успешно удалён."
        });

        response.TraceId = HttpContext.TraceIdentifier;

        return Ok(response);
    }
}