public class JobExamSubmissionDto
{
    public int Id { get; set; } // معرف المحاولة

    public int JobExamId { get; set; } // الامتحان الذي تم حله

    public int UserProfileId { get; set; } // المستخدم الذي حل الامتحان

    public DateTime SubmittedAt { get; set; } // تاريخ التسليم
}