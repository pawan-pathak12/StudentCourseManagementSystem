using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Tests.Unit.TestUtils.Builders
{
    internal class CourseBuilder
    {
        // Private fields with default values
        private int _courseId = 1;
        private string _code = "CSE101";
        private string _title = "Introduction to Computer Science";
        private int _credits = 3;
        private string _description = "Default course description";
        private string _instructor = "Default Instructor";
        private DateTimeOffset _startDate = DateTimeOffset.UtcNow;
        private DateTimeOffset _endDate = DateTimeOffset.UtcNow.AddMonths(3);
        private bool _isActive = true;

        private int _capacity = 30;
        private DateTimeOffset _enrollmentStartDate = DateTimeOffset.UtcNow;
        private DateTimeOffset _enrollmentEndDate = DateTimeOffset.UtcNow.AddDays(30);


        #region  Builder methods
        public CourseBuilder WithCourseId(int courseId)
        {
            _courseId = courseId;
            return this;
        }

        public CourseBuilder WithCode(string code)
        {
            _code = code;
            return this;
        }

        public CourseBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public CourseBuilder WithCredits(int credits)
        {
            _credits = credits;
            return this;
        }

        public CourseBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public CourseBuilder WithInstructor(string instructor)
        {
            _instructor = instructor;
            return this;
        }

        public CourseBuilder WithStartDate(DateTimeOffset startDate)
        {
            _startDate = startDate;
            return this;
        }

        public CourseBuilder WithEndDate(DateTimeOffset endDate)
        {
            _endDate = endDate;
            return this;
        }

        public CourseBuilder WithIsActive(bool isActive)
        {
            _isActive = isActive;
            return this;
        }

        public CourseBuilder WithCapacity(int capacity)
        {
            _capacity = capacity;
            return this;
        }

        public CourseBuilder WithEnrollmentStartDate(DateTimeOffset enrollmentStartDate)
        {
            _enrollmentStartDate = enrollmentStartDate;
            return this;
        }

        public CourseBuilder WithEnrollmentEndDate(DateTimeOffset enrollmentEndDate)
        {
            _enrollmentEndDate = enrollmentEndDate;
            return this;
        }

        #endregion

        // Build method to create complete Course entity
        public Course Build()
        {
            return new Course
            {
                CourseId = _courseId,
                Code = _code,
                Title = _title,
                Credits = _credits,
                Description = _description,
                Instructor = _instructor,
                StartDate = _startDate,
                EndDate = _endDate,
                IsActive = _isActive,
                Capacity = _capacity,
                EnrollmentStartDate = _enrollmentStartDate,
                EnrollmentEndDate = _enrollmentEndDate
            };
        }
    }
}