using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.Refunds
{
    [TestClass]
    internal class ProcessRefundTests : RefundServiceBaseClass
    {
        //process refund return tru  
        [TestMethod]
        public async Task ProcessRefundAsync_ShouldreturnTrue_WhenItPassesAllRules()
        {

        }

        // valid logic 
        [TestMethod]
        public async Task ProcessRefundAsync_ShouldPass_ValidEligibility()
        {

        }
        //add refund pamynet 
        [TestMethod]
        public async Task ProcessRefundAsync_ShouldCreateNegativePayment()
        {

        }

        //update payment status 
        [TestMethod]
        public async Task ProcessRefundAsync_ShouldUpdateOrginalPaymentStatus()
        {

        }

        //update invocie 
        [TestMethod]
        public async Task ProcessRefundAsync_ShouldUpdatesInvoiceBalance()
        {

        }

        // update feeass
        [TestMethod]
        public async Task ProcessRefundAsync_ShouldUpdatesfeeAssessment_WhenInvoiceWasPaid()
        {

        }



    }
}
