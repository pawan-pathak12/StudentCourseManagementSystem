namespace StudentCourseManagement.Domain.Constants
{
    public static class FinancialConstants
    {
        static int DUE_DATE_DAYS = 30;              // for invoiceDue Date 
        static decimal LATE_FEE_PERCENTAGE = 0.10m; // for late fee calculation 
        static int REFUND_WINDOW_DAYS = 30;        //for refund eligibility 
        static int COURSE_START_BUFFER_DAYS = 2;  //for refund eligibility 
    }
}
