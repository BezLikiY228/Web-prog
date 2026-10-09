
using Microsoft.AspNetCore.Mvc;
using UniversityApi.Common;
using UniversityApi.DTO.Enrollments;
using UniversityApi.Services.Interfaces;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _service;

    public EnrollmentsController(IEnrollmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ReturnResult<List<EnrollmentDto>>>> GetAll()
    {
        var enrollments = await _service.GetAllAsync();

        var response = ReturnResult<List<EnrollmentDto>>.Success(enrollments);
        response.TraceId = HttpContext.TraceIdentifier;

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReturnResult<EnrollmentDto>>> GetById(int id)
    {
        var enrollment = await _service.GetByIdAsync(id);

        if (enrollment == null)
        {
            return NotFound(ReturnResult<EnrollmentDto>.Failure(
                404,
                "ENROLLMENT_NOT_FOUND",
                "Запись на курс не найдена.",
                HttpContext.TraceIdentifier));
        }

        var response = ReturnResult<EnrollmentDto>.Success(enrollment);
        response.TraceId = HttpContext.TraceIdentifier;

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ReturnResult<EnrollmentDto>>> Create(
        EnrollmentCreateDto dto)
    {
        try
        {
            var enrollment = await _service.CreateAsync(dto);

            var response = ReturnResult<EnrollmentDto>.Success(enrollment, 201);
            response.TraceId = HttpContext.TraceIdentifier;

            return StatusCode(201, response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ReturnResult<EnrollmentDto>.Failure(
                409,
                "ENROLLMENT_CONFLICT",
                ex.Message,
                HttpContext.TraceIdentifier));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ReturnResult<EnrollmentDto>>> Update(
        int id,
        EnrollmentUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
        {
            return NotFound(ReturnResult<EnrollmentDto>.Failure(
                404,
                "ENROLLMENT_NOT_FOUND",
                "Запись на курс не найдена.",
                HttpContext.TraceIdentifier));
        }

        var enrollment = await _service.GetByIdAsync(id);

        var response = ReturnResult<EnrollmentDto>.Success(enrollment!);
        response.TraceId = HttpContext.TraceIdentifier;

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ReturnResult<object>>> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(ReturnResult<object>.Failure(
                404,
                "ENROLLMENT_NOT_FOUND",
                "Запись на курс не найдена.",
                HttpContext.TraceIdentifier));
        }

        var response = ReturnResult<object>.Success(new
        {
            Message = "Запись на курс успешно удалена."
        });

        response.TraceId = HttpContext.TraceIdentifier;

        return Ok(response);
    }
}