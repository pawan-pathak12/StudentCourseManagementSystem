using System.ComponentModel.DataAnnotations;

namespace StudentCourseManagement.Business.DTOs.Courses
{
    public class UpdateCourseDto
    {
        public int CourseId { get; set; }   // Required for identifying which course to update
        public string Code { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Credits { get; set; }
        public string Description { get; set; }
        [Required]
        public string Instructor { get; set; }
        [Required]
        public DateTimeOffset StartDate { get; set; }


        public DateTimeOffset EndDate { get; set; }



        public int Capacity { get; set; }
        public DateTimeOffset EnrollmentStartDate { get; set; }
        public DateTimeOffset EnrollmentEndDate { get; set; }


        public bool IsActive { get; set; }  // Allow toggling active status
    }


}

