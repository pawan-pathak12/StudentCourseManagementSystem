using AutoMapper;
using StudentCourseManagement.API.DTOs;
using StudentCourseManagement.Business.DTOs.Courses;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Mapping
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, Course>();
            CreateMap<Course, CreateCourseDto>().ReverseMap();
            CreateMap<Course, UpdateCourseDto>().ReverseMap();
            CreateMap<CourseResponseDto, Course>().ReverseMap();

        }
    }
}
