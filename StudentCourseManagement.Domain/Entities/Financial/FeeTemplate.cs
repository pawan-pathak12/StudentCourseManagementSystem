using Student_Course_Management_API.Enums;

namespace Student_Course_Management_API.Models;

public class FeeTemplate
{
    public int FeeTemplateId { get; set; }
    public string? Name { get; set; }
    public int CourseId { get; set; }
    public CalculationType CalculationType { get; set; }
    public decimal Amount { get; set; }
    public decimal RatePerCredit { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    // for join queries required 
    public string CourseTitle { get; set; }
}