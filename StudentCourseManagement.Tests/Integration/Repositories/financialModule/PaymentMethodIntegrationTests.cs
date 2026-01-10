using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Data.Repositories.Dapper.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using System.Transactions;

namespace StudentCourseManagement.Tests.Integration.Repositories.financialModule
{
    [TestClass]
    public class PaymentMethodIntegrationTests
    {
        private readonly PaymentMethodRepository _paymentMethodRepository;
        public PaymentMethodIntegrationTests()
        {
            var dbfixture = new DatabaseFixture();
            var mockLogger = new Mock<ILogger<PaymentMethodRepository>>();
            _paymentMethodRepository = new PaymentMethodRepository(dbfixture.DbContext, mockLogger.Object);
        }

        #region CURD Operations 
        [TestMethod]
        public async Task AddAsync_WithValid_InsertsRowAndReturnsId()
        {
            using var tanscationScoped = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var paymentMethodId = await CreatePaymentMethodAsync();

            Assert.IsTrue(paymentMethodId > 0);

        }

        [TestMethod]
        public async Task GettAllAsync_IfNotNullThen_ReturnListOf()
        {
            //Arrange 
            using var tanscationScoped = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            await CreatePaymentMethodAsync();

            //Act 
            var payments = await _paymentMethodRepository.GetAllAsync();

            //Assert 
            Assert.IsTrue(payments.Any());
        }


        [TestMethod]
        public async Task UpdateAsync_WithExistingId_ReturnsTrue_AndUpdatesData()
        {
            //Arrange 
            using var tanscationScoped = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var paymentMethodId = await CreatePaymentMethodAsync();

            var updated = new PaymentMethod
            {
                PaymentMethodId = paymentMethodId,
                IsActive = true,
                PaymentMethodType = PaymentMethodType.Cash,
                Provider = "New Updated Provider"
            };


            //Act 
            var result = await _paymentMethodRepository.UpdateAsync(paymentMethodId, updated);

            //Assert 
            Assert.IsTrue(result);
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(paymentMethodId);

            Assert.AreEqual(updated.Provider, paymentMethod?.Provider);
        }

        [TestMethod]
        public async Task DeleteAsync_WithActiveId_SetsIsActiveFalse()
        {
            //Arrange 
            using var tanscationScoped = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var paymentMethodId = await CreatePaymentMethodAsync();

            //Act 
            var result = await _paymentMethodRepository.DeleteAsync(paymentMethodId);

            //Assert     
            Assert.IsTrue(result);
        }

        #endregion

        #region Private hepler methods 
        private async Task<int> CreatePaymentMethodAsync()
        {
            var paymentMethod = new PaymentMethod
            {
                PaymentMethodType = PaymentMethodType.Cash,
                Provider = "nothing",
                IsActive = true
            };

            return await _paymentMethodRepository.AddAsync(paymentMethod);
        }
        #endregion
    }
}
