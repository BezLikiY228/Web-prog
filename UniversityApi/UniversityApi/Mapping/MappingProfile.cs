using AutoMapper;
using UniversityApi.Models;
using UniversityApi.DTO.Students;
using UniversityApi.DTO.Teachers;
using UniversityApi.DTO.Courses;
using UniversityApi.DTO.Enrollments;

namespace UniversityApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Student, StudentDto>();
        CreateMap<StudentCreateDto, Student>();
        CreateMap<StudentUpdateDto, Student>();

        CreateMap<Teacher, TeacherDto>();
        CreateMap<TeacherCreateDto, Teacher>();
        CreateMap<TeacherUpdateDto, Teacher>();

        CreateMap<Course, CourseDto>()
            .ForMember(
                dest => dest.TeacherName,
                opt => opt.MapFrom(
                    src => src.Teacher == null
                        ? null
                        : src.Teacher.FirstName + " " + src.Teacher.LastName));

        CreateMap<CourseCreateDto, Course>();
        CreateMap<CourseUpdateDto, Course>();

        CreateMap<Enrollment, EnrollmentDto>()
            .ForMember(
                dest => dest.StudentName,
                opt => opt.MapFrom(
                    src => src.Student == null
                        ? null
                        : src.Student.FirstName + " " + src.Student.LastName))
            .ForMember(
                dest => dest.CourseName,
                opt => opt.MapFrom(
                    src => src.Course == null ? null : src.Course.Name));

        CreateMap<EnrollmentCreateDto, Enrollment>();
        CreateMap<EnrollmentUpdateDto, Enrollment>();
    }
}