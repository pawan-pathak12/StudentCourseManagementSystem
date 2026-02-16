namespace StudentCourseManagement.Tests.Api.Builders
{
    public class PaymentTestData
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int EnrollmentId { get; set; }
        public int FeeAssessmentId { get; set; }
        public int InvoiceId { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal InvoiceAmount { get; set; }
    }
}
