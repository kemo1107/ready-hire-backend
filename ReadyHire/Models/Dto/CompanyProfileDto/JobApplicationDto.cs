public class JobApplicationDto
{
    public int Id { get; set; } // معرف التقديم

    public int JobId { get; set; } // الوظيفة المقدمة لها

    public int UserProfileId { get; set; } // المستخدم الذي قدم

    public bool HasPassedExam { get; set; } // هل نجح في الامتحان؟

    public DateTime AppliedAt { get; set; } // تاريخ التقديم

    public double MatchRatio { get; set; }

}
