using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class JobQuestion
{
    [Key]
    public int Id { get; set; } // المفتاح الأساسي للسؤال

    [Required]
    public int JobExamId { get; set; } // الامتحان اللي السؤال جزء منه

    [ForeignKey(nameof(JobExamId))]
    public JobExam JobExam { get; set; } = null!; // ربط بالامتحان

    [Required]
    public string QuestionText { get; set; } = null!; // نص السؤال

    [Required]
    public List<string> Choices { get; set; } = new(); // الاختيارات

    [Required]
    public string CorrectAnswer { get; set; } = null!; // الإجابة الصحيحة
}