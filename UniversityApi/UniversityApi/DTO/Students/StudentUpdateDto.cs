using System.ComponentModel.DataAnnotations;

namespace UniversityApi.DTO.Students;

public class StudentUpdateDto
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }
}