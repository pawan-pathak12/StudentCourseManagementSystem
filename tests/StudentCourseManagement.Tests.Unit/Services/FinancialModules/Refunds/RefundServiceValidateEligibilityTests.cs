using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.Refunds
{
    [TestClass]
    public class RefundServiceValidateEligibilityTests : RefundServiceBaseClass
    {
        [TestMethod]
        public async Task ValidateEligibilityAsync_ShouldReturnFalse_WhenPaymentNotFound()
        {
        }

        [TestMethod]
        public async Task ValidateEligibilityAsync_ShouldReturnFalse_WhenPaymentStatusIsNotCompleted()
        {
        }

        [TestMethod]
        public async Task ValidateEligibilityAsync_ShouldReturnFalse_WhenPaymentIsAlreadyRefunded()
        {
        }

        [TestMethod]
        public async Task ValidateEligibilityAsync_ShouldReturnFalse_WhenRefundWindowHasExpired()
        {
        }

        [TestMethod]
        public async Task ValidateEligibilityAsync_ShouldReturnFalse_WhenCourseStartedTooLongAgo()
        {
        }

        [TestMethod]
        public async Task ValidateEligibility_ShouldReturnTrue_WhenAllRulesPass()
        {

        }
    }
}
