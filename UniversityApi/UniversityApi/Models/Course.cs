using System.ComponentModel.DataAnnotations;

namespace UniversityApi.Models;

public class Course
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Range(1, 30)]
    public int Credits { get; set; }

    public int TeacherId { get; set; }

    public Teacher? Teacher { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Enrollment> Enrollments { get; set; }
        = new List<Enrollment>();
}