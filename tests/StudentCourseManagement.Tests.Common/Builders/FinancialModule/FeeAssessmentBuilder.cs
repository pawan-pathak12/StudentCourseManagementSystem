using StudentCourseManagement.Domain.Constants;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Tests.Common.Builders.FinancialModule
{
    public class FeeAssessmentBuilder
    {
        private int _feeAssessmentId;
        private int _enrollmentId;
        private int _courseId;
        private int _feeTemplateId;
        private decimal _amount;
        private DateTimeOffset? _dueDate = DateTimeOffset.UtcNow.AddDays(FinancialConstants.DUE_DATE_DAYS);
        private AssessmentStatus _feeAssessmentStatus = AssessmentStatus.Assessed;
        private bool _isActive = true;
        private DateTimeOffset _assessmentDate = DateTimeOffset.UtcNow;
        private DateTimeOffset? _paidDate;              // When fully paid
        private decimal? _lateFeeAmount = 0;                // Late fee amount (10%)
        private DateTimeOffset? _lateFeeAppliedDate = null;    // When late fee was added

        #region ..With Method 
        public FeeAssessmentBuilder WithFeeAssessmentId(int feeAssessmentId)
        {
            _feeAssessmentId = feeAssessmentId;
            return this;
        }

        public FeeAssessmentBuilder WithEnrollmentId(int enrollmentId)
        {
            _enrollmentId = enrollmentId;
            return this;
        }

        public FeeAssessmentBuilder WithCourseId(int courseId)
        {
            _courseId = courseId;
            return this;
        }

        public FeeAssessmentBuilder WithFeeTemplateId(int feeTemplateId)
        {
            _feeTemplateId = feeTemplateId;
            return this;
        }

        public FeeAssessmentBuilder WithAmount(decimal amount)
        {
            _amount = amount;
            return this;
        }

        public FeeAssessmentBuilder WithDueDate(DateTimeOffset? dueDate)
        {
            _dueDate = dueDate;
            return this;
        }

        public FeeAssessmentBuilder WithFeeAssessmentStatus(AssessmentStatus status)
        {
            _feeAssessmentStatus = status;
            return this;
        }

        public FeeAssessmentBuilder WithIsActive(bool isActive)
        {
            _isActive = isActive;
            return this;
        }

        public FeeAssessmentBuilder WithAssessmentDate(DateTimeOffset assessmentDate)
        {
            _assessmentDate = assessmentDate;
            return this;
        }

        public FeeAssessmentBuilder WithPaidDate(DateTimeOffset? paidDate)
        {
            _paidDate = paidDate;
            return this;
        }

        public FeeAssessmentBuilder WithLateFeeAmount(decimal? lateFeeAmount)
        {
            _lateFeeAmount = lateFeeAmount;
            return this;
        }

        public FeeAssessmentBuilder WithLateFeeAppliedDate(DateTimeOffset? lateFeeAppliedDate)
        {
            _lateFeeAppliedDate = lateFeeAppliedDate;
            return this;
        }
        #endregion

        public FeeAssessment Build()
        {
            return new FeeAssessment
            {
                EnrollmentId = _enrollmentId,
                CourseId = _courseId,
                FeeTemplateId = _feeTemplateId,
                Amount = _amount,
                DueDate = _dueDate,
                FeeAssessmentStatus = _feeAssessmentStatus,
                IsActive = _isActive,
                AssessmentDate = _assessmentDate,
                PaidDate = _paidDate,
                LateFeeAmount = _lateFeeAmount,
                LateFeeAppliedDate = _lateFeeAppliedDate
            };
        }
    }
}