public class JobQuestionDto
{
    public int Id { get; set; } // معرف السؤال

    public int JobExamId { get; set; } // الامتحان الذي ينتمي له السؤال

    public string QuestionText { get; set; } = null!; // نص السؤال

    public List<string> Choices { get; set; } = new(); // الاختيارات

    public string CorrectAnswer { get; set; } = null!; // الإجابة الصحيحة
}
