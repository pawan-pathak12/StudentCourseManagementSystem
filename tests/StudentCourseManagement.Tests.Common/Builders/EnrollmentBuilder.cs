using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Tests.Common.Builders
{
    public class EnrollmentBuilder
    {
        private int _enrollmentId = 1;
        private DateTimeOffset _enrollmentDate = DateTimeOffset.UtcNow;
        private int _studentId = 1;
        private int _courseId = 1;
        private EnrollmentStatus _enrollmentStatus = EnrollmentStatus.Comfirmed;
        private bool _isActive = true;
        private DateTimeOffset _createdAt = DateTimeOffset.UtcNow;
        private DateTimeOffset? _feeAssessmentDate = DateTimeOffset.UtcNow;
        private DateTimeOffset? _cancelledDate;
        private string? _cancellationReason;


        #region Method
        public EnrollmentBuilder WithEnrollmentId(int enrollmentId)
        {
            _enrollmentId = enrollmentId;
            return this;
        }

        public EnrollmentBuilder WithEnrollmentDate(DateTimeOffset enrollmentDate)
        {
            _enrollmentDate = enrollmentDate;
            return this;
        }

        public EnrollmentBuilder WithStudentId(int studentId)
        {
            _studentId = studentId;
            return this;
        }

        public EnrollmentBuilder WithCourseId(int courseId)
        {
            _courseId = courseId;
            return this;
        }

        public EnrollmentBuilder WithEnrollmentStatus(EnrollmentStatus status)
        {
            _enrollmentStatus = status;
            return this;
        }


        public EnrollmentBuilder WithIsActive(bool isActive)
        {
            _isActive = isActive;
            return this;
        }

        public EnrollmentBuilder WithCreatedAt(DateTimeOffset createdAt)
        {
            _createdAt = createdAt;
            return this;
        }

        public EnrollmentBuilder WithFeeAssessmentDate(DateTimeOffset? feeAssessmentDate)
        {
            _feeAssessmentDate = feeAssessmentDate;
            return this;
        }

        public EnrollmentBuilder WithCancelledDate(DateTimeOffset? cancelledDate)
        {
            _cancelledDate = cancelledDate;
            return this;
        }

        public EnrollmentBuilder WithCancellationReason(string? reason)
        {
            _cancellationReason = reason;
            return this;
        }
        #endregion

        public Enrollment Build()
        {
            return new Enrollment
            {
                EnrollmentDate = _enrollmentDate,
                StudentId = _studentId,
                CourseId = _courseId,
                EnrollmentStatus = _enrollmentStatus,
                IsActive = _isActive,
                CreatedAt = _createdAt,
            };
        }

    }
}
