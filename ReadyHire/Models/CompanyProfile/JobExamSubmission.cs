using ReadyHire.Models.UserProfile;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class JobExamSubmission
{
    [Key]
    public int Id { get; set; } // المفتاح الأساسي لمحاولة الامتحان

    [Required]
    public int JobExamId { get; set; } // الامتحان اللي تم حله

    [ForeignKey(nameof(JobExamId))]
    public JobExam JobExam { get; set; } = null!; // الربط بالامتحان

    [Required]
    public int UserProfileId { get; set; } // المتقدم اللي حل الامتحان

    [ForeignKey(nameof(UserProfileId))]
    public UserProfiles UserProfile { get; set; } = null!; // الربط بالبروفايل

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow; // تاريخ تسليم الامتحان

    public ICollection<JobExamAnswer> Answers { get; set; } = new List<JobExamAnswer>(); // الإجابات اللي حلها
}
