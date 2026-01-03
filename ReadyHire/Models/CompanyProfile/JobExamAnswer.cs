using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class JobExamAnswer
{
    [Key]
    public int Id { get; set; } // المفتاح الأساسي للإجابة

    [Required]
    public int JobExamSubmissionId { get; set; } // المحاولة اللي ينتمي إليها الجواب

    [ForeignKey(nameof(JobExamSubmissionId))]
    public JobExamSubmission JobExamSubmission { get; set; } = null!; // الربط بالمحاولة

    [Required]
    public int QuestionId { get; set; } // السؤال اللي تم حله

    [ForeignKey(nameof(QuestionId))]
    public JobQuestion JobQuestion { get; set; } = null!; // الربط بالسؤال نفسه

    [Required]
    public string SelectedAnswer { get; set; } = null!; // الإجابة المختارة
}

