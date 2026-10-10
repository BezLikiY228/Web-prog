namespace UniversityApi.DTO.Enrollments;

public class EnrollmentDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string? StudentName { get; set; }
    public int CourseId { get; set; }
    public string? CourseName { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public decimal? Grade { get; set; }
}