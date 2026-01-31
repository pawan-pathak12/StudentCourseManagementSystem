using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Data.Repositories.InMemory
{
    public class InMemoryDbContext
    {
        public List<Student> Students { get; } = new();
        public List<Course> Courses { get; } = new();
        public List<Enrollment> Enrollments { get; } = new();

        public List<FeeAssessment> FeeAssessments { get; } = new();
        public List<FeeTemplate> feeTemplates { get; } = new();
        public List<Invoice> Invoices { get; } = new();
        public List<InvoiceLineItem> LineItems { get; set; } = new();
        public List<Payment> Payments { get; } = new();
        public List<PaymentMethod> PaymentMethods { get; } = new();
    }
}
