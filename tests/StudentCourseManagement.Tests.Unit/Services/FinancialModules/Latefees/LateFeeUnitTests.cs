using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.Latefees
{
    [TestClass]
    public class LateFeeUnitTests : LatefeeServiceTestBaseClass
    {
        #region ApplyLatefee
        [TestMethod]
        public async Task ApplyLateFeeAsync_ShouldReturnTrue_WhenAllValidationIsPassed()
        {
            //Arrange 
            //Act 
            //Assert 
        }

        [TestMethod]
        public async Task ApplyLateFeeAsync_ShouldReturnFalse_WhenOverDueInvoiceNotFound()
        {
            //Arrange 
            //Act 
            //Assert 
        }

        [TestMethod]
        public async Task ApplyLateFeeAsync_ShouldReturnFalse_InvoiceNotFound()
        {
            //Arrange 
            //Act 
            //Assert 
        }
        [TestMethod]
        public async Task ApplyLateFeeAsync_ShouldReturnFalse_WhenIsNotPayable()
        {
            //Arrange 
            //Act 
            //Assert 
        }
        [TestMethod]
        public async Task ApplyLateFeeAsync_ShouldReturnFalse_WhenLateFeeIsALreadyApplied()
        {
            //Arrange 
            //Act 
            //Assert 
        }

        [TestMethod]
        public async Task ApplyLateFee_CalculatesCorrectly()
        {
            //Arrange 
            //Act 
            //Assert 
        }

        [TestMethod]
        public async Task ApplyLateFeeAsync_ShouldUpdate_InvoiceSucessfully()
        {
            //Arrange 
            //Act 
            //Assert 
        }
        [TestMethod]
        public async Task ApplyLateFeeAsync_ShouldUpdate_FeeAssessmentSucessfully()
        {
            //Arrange 
            //Act 
            //Assert 
        }
        [TestMethod]
        public async Task ApplyLateFeeAsync_ShouldCreate_InvoiceLineItem()
        {
            //Arrange 
            //Act 
            //Assert 
        }

        #endregion

        #region ProcessAllOverDue

        [TestMethod]
        public async Task ProcessAllOverdue_ShouldReturnZero_WhenNoOverDueInvoices()
        {
            //Arrange 
            //Act 
            //Assert 
        }

        [TestMethod]
        public async Task ProcessAllOverdue_ShouldProcessAll_WhenMultipleInvoiceProvided()
        {
            //Arrange 
            //Act 
            //Assert 
        }
        #endregion
    }
}
