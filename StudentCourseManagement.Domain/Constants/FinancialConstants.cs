namespace StudentCourseManagement.Domain.Constants
{
    public static class FinancialConstants
    {
        public const int DUE_DATE_DAYS = 30;              // for invoicedue date
        public const decimal LATE_FEE_PERCENTAGE = 0.10m; // for latefee calculation
        public const int REFUND_WINDOW_DAYS = 30;         // for refund eligibility
        public const int COURSE_START_BUFFER_DAYS = 2;    // for refund eligibility
    }
}
