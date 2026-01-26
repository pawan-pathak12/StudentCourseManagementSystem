using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Tests.Common.Builders.FinancialModule
{
    public class InvoiceLineItemBuilder
    {
        private int _invoiceLineItemId;
        private int _invoiceId;
        private int _feeTemplateId;
        private int _courseId;
        private bool _isActive = true;
        private string? _description;
        private int _quantity = 1;
        private decimal _unitPrice;
        private decimal _amount;
        private DateTimeOffset _createdAt = DateTimeOffset.UtcNow;

        public InvoiceLineItemBuilder WithInvoiceLineItemId(int invoiceLineItemId)
        {
            _invoiceLineItemId = invoiceLineItemId;
            return this;
        }

        public InvoiceLineItemBuilder WithInvoiceId(int invoiceId)
        {
            _invoiceId = invoiceId;
            return this;
        }

        public InvoiceLineItemBuilder WithFeeTemplateId(int feeTemplateId)
        {
            _feeTemplateId = feeTemplateId;
            return this;
        }

        public InvoiceLineItemBuilder WithCourseId(int courseId)
        {
            _courseId = courseId;
            return this;
        }

        public InvoiceLineItemBuilder WithIsActive(bool isActive)
        {
            _isActive = isActive;
            return this;
        }

        public InvoiceLineItemBuilder WithDescription(string? description)
        {
            _description = description;
            return this;
        }

        public InvoiceLineItemBuilder WithQuantity(int quantity)
        {
            _quantity = quantity;
            return this;
        }

        public InvoiceLineItemBuilder WithUnitPrice(decimal unitPrice)
        {
            _unitPrice = unitPrice;
            return this;
        }

        public InvoiceLineItemBuilder WithAmount(decimal amount)
        {
            _amount = amount;
            return this;
        }

        public InvoiceLineItemBuilder WithCreatedAt(DateTimeOffset createdAt)
        {
            _createdAt = createdAt;
            return this;
        }

        public InvoiceLineItem Build()
        {
            return new InvoiceLineItem
            {
                InvoiceLineItemId = _invoiceLineItemId,
                InvoiceId = _invoiceId,
                FeeTemplateId = _feeTemplateId,
                CourseId = _courseId,
                IsActive = _isActive,
                Description = _description,
                Quantity = _quantity,
                UnitPrice = _unitPrice,
                Amount = _amount,
                CreatedAt = _createdAt
            };
        }
    }
}
