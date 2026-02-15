namespace StudentCourseManagement.Application.DTOs.FInancialModule.InvoiceLineItems
{
    public class UpdateInvoiceLineItemDto
    {
        public int LineItemId { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
    }

}
