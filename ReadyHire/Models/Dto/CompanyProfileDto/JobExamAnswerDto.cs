public class JobExamAnswerDto
{
    public int Id { get; set; } // معرف الإجابة

    public int JobExamSubmissionId { get; set; } // المحاولة التي ينتمي لها الجواب

    public int QuestionId { get; set; } // معرف السؤال

    public string SelectedAnswer { get; set; } = null!; // الإجابة المختارة
}