using ReadyHire.Models.CompanyProfile;
using ReadyHire.Models.UserProfile;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class JobApplication
{
    [Key]
    public int Id { get; set; } // المفتاح الأساسي للتقديم

    [Required]
    public int JobId { get; set; } // الوظيفة اللي مقدم عليها

    [ForeignKey(nameof(JobId))]
    public Job Job { get; set; } = null!; // الربط بالوظيفة

    [Required]
    public int UserProfileId { get; set; } // المستخدم اللي بيقدم (البروفايل)

    [ForeignKey(nameof(UserProfileId))]
    public UserProfiles UserProfile { get; set; } = null!; // الربط بالبروفايل

    public bool HasPassedExam { get; set; } = false; // هل نجح في امتحان الوظيفة؟

    public DateTime AppliedAt { get; set; } = DateTime.UtcNow; // تاريخ التقديم

    public double MatchRatio { get; set; }

    public string ApplicationStatus { get; set; } = "Under Review";

}