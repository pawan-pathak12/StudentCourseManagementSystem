using AutoMapper;
using StudentCourseManagement.Application.DTOs.DTOs.Enrollments;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Mapping
{
    public class EnrollmentProfile : Profile
    {
        public EnrollmentProfile()
        {
            CreateMap<Enrollment, Enrollment>();
            CreateMap<Enrollment, CreateEnrollmentDto>().ReverseMap();
            CreateMap<Enrollment, UpdateEnrollmentDto>().ReverseMap();
            CreateMap<Enrollment, EnrollmentResponseDto>().ReverseMap();
        }
    }
}
