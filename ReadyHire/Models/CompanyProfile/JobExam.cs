using ReadyHire.Models.CompanyProfile;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class JobExam
{
    [Key]
    public int Id { get; set; } // المفتاح الأساسي للامتحان

    [Required]
    public int JobId { get; set; } // الوظيفة اللي الامتحان تبعها

    [ForeignKey(nameof(JobId))]
    public Job Job { get; set; } = null!; // ربط بالوظيفة

    [Required]
    public string Title { get; set; } = null!; // عنوان الامتحان

    public List<JobQuestion> Questions { get; set; } = new(); // الأسئلة المرتبطة بالامتحان
}