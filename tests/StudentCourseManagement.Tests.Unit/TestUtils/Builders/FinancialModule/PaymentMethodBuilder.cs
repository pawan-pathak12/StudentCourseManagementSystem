using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Tests.Unit.TestUtils.Builders.FinancialModule
{
    public class PaymentMethodBuilder
    {
        private int _paymentMethodId;
        private string? _name;
        private PaymentMethodType _paymentMethodType;
        private bool _isActive = true;

        #region WithXX... Methods

        public PaymentMethodBuilder WithPaymentMethodId(int paymentMethodId)
        {
            _paymentMethodId = paymentMethodId;
            return this;
        }

        public PaymentMethodBuilder WithName(string? name)
        {
            _name = name;
            return this;
        }

        public PaymentMethodBuilder WithPaymentMethodType(PaymentMethodType paymentMethodType)
        {
            _paymentMethodType = paymentMethodType;
            return this;
        }

        public PaymentMethodBuilder WithIsActive(bool isActive)
        {
            _isActive = isActive;
            return this;
        }

        #endregion

        public PaymentMethod Build()
        {
            return new PaymentMethod
            {
                PaymentMethodId = _paymentMethodId,
                Name = _name,
                PaymentMethodType = _paymentMethodType,
                IsActive = _isActive
            };
        }
    }
}
