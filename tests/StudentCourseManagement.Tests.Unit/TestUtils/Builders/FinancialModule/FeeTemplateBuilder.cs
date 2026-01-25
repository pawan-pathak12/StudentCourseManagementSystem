
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Tests.Unit.TestUtils.Builders.FinancialModule
{
    public class FeeTemplateBuilder
    {
        private int _feeTemplateId;
        private int _courseId;
        private string? _name;
        private CalculationType _calculationType;
        private decimal _amount;
        private decimal _ratePerCredit;
        private bool _isActive = true;
        private DateTimeOffset _createdAt = DateTimeOffset.UtcNow;
        private DateTimeOffset? _updatedAt;

        #region ..With Method
        public FeeTemplateBuilder WithFeeTemplateId(int feeTemplateId)
        {
            _feeTemplateId = feeTemplateId;
            return this;
        }

        public FeeTemplateBuilder WithCourseId(int courseId)
        {
            _courseId = courseId;
            return this;
        }

        public FeeTemplateBuilder WithName(string? name)
        {
            _name = name;
            return this;
        }

        public FeeTemplateBuilder WithCalculationType(CalculationType calculationType)
        {
            _calculationType = calculationType;
            return this;
        }

        public FeeTemplateBuilder WithAmount(decimal amount)
        {
            _amount = amount;
            return this;
        }

        public FeeTemplateBuilder WithRatePerCredit(decimal ratePerCredit)
        {
            _ratePerCredit = ratePerCredit;
            return this;
        }

        public FeeTemplateBuilder WithIsActive(bool isActive)
        {
            _isActive = isActive;
            return this;
        }

        public FeeTemplateBuilder WithCreatedAt(DateTimeOffset createdAt)
        {
            _createdAt = createdAt;
            return this;
        }

        public FeeTemplateBuilder WithUpdatedAt(DateTimeOffset? updatedAt)
        {
            _updatedAt = updatedAt;
            return this;
        }
        #endregion

        public FeeTemplate Build()
        {
            return new FeeTemplate
            {
                FeeTemplateId = _feeTemplateId,
                CourseId = _courseId,
                Name = _name,
                CalculationType = _calculationType,
                Amount = _amount,
                RatePerCredit = _ratePerCredit,
                IsActive = _isActive,
                CreatedAt = _createdAt,
                UpdatedAt = _updatedAt
            };
        }
    }
}
