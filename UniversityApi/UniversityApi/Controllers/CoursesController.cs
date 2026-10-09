
using Microsoft.AspNetCore.Mvc;
using UniversityApi.Common;
using UniversityApi.DTO.Courses;
using UniversityApi.Services.Interfaces;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _service;

    public CoursesController(ICourseService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ReturnResult<List<CourseDto>>>> GetAll(
        [FromQuery] int? teacherId,
        [FromQuery] int? minCredits,
        [FromQuery] int? maxCredits)
    {
        if (minCredits.HasValue && minCredits.Value < 1 ||
            maxCredits.HasValue && maxCredits.Value > 30 ||
            minCredits.HasValue && maxCredits.HasValue &&
            minCredits.Value > maxCredits.Value)
        {
            return BadRequest(ReturnResult<List<CourseDto>>.Failure(
                400,
                "INVALID_FILTER",
                "Проверьте значения фильтров кредитов.",
                HttpContext.TraceIdentifier));
        }

        var courses = await _service.GetAllAsync(
            teacherId, minCredits, maxCredits);

        var response = ReturnResult<List<CourseDto>>.Success(courses);
        response.TraceId = HttpContext.TraceIdentifier;

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReturnResult<CourseDto>>> GetById(int id)
    {
        var course = await _service.GetByIdAsync(id);

        if (course == null)
        {
            return NotFound(ReturnResult<CourseDto>.Failure(
                404,
                "COURSE_NOT_FOUND",
                "Курс не найден.",
                HttpContext.TraceIdentifier));
        }

        var response = ReturnResult<CourseDto>.Success(course);
        response.TraceId = HttpContext.TraceIdentifier;

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ReturnResult<CourseDto>>> Create(
        CourseCreateDto dto)
    {
        try
        {
            var course = await _service.CreateAsync(dto);

            var response = ReturnResult<CourseDto>.Success(course, 201);
            response.TraceId = HttpContext.TraceIdentifier;

            return StatusCode(201, response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ReturnResult<CourseDto>.Failure(
                409,
                "COURSE_CONFLICT",
                ex.Message,
                HttpContext.TraceIdentifier));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ReturnResult<CourseDto>>> Update(
        int id,
        CourseUpdateDto dto)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(ReturnResult<CourseDto>.Failure(
                    404,
                    "COURSE_NOT_FOUND",
                    "Курс не найден.",
                    HttpContext.TraceIdentifier));
            }

            var course = await _service.GetByIdAsync(id);

            var response = ReturnResult<CourseDto>.Success(course!);
            response.TraceId = HttpContext.TraceIdentifier;

            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ReturnResult<CourseDto>.Failure(
                409,
                "COURSE_CONFLICT",
                ex.Message,
                HttpContext.TraceIdentifier));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ReturnResult<object>>> Delete(int id)
    {
        try
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(ReturnResult<object>.Failure(
                    404,
                    "COURSE_NOT_FOUND",
                    "Курс не найден.",
                    HttpContext.TraceIdentifier));
            }

            var response = ReturnResult<object>.Success(new
            {
                Message = "Курс успешно удалён."
            });

            response.TraceId = HttpContext.TraceIdentifier;

            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ReturnResult<object>.Failure(
                409,
                "COURSE_CONFLICT",
                ex.Message,
                HttpContext.TraceIdentifier));
        }
    }
}