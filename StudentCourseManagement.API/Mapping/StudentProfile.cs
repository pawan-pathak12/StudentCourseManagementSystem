using AutoMapper;
using StudentCourseManagement.Application.DTOs.DTOs.Students;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Mapping
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, CreateStudentDto>().ReverseMap();
            CreateMap<Student, StudentResponseDto>().ReverseMap();
            CreateMap<Student, UpdateStudentDto>().ReverseMap();
        }

    }
}
