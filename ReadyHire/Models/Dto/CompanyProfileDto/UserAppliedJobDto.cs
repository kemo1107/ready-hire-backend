public class UserAppliedJobDto
{
    public string JobTitle { get; set; }
    public string Type { get; set; } // Full-Time, Freelance, etc.
    public DateTime AppliedAt { get; set; }
    public string Status { get; set; } // ApplicationStatus (Accepted, Rejected, Under Review)
}
