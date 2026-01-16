using AutoMapper;
using StudentCourseManagement.API.DTOs;
using StudentCourseManagement.Business.DTOs.Student;
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
