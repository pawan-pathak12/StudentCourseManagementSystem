using AutoMapper;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Mapping
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, Course>();

        }
    }
}
