namespace ReadyHire.Models.Dto.CompanyProfileDto
{
    public class JobExamDto
    {
        public int Id { get; set; } // معرف الامتحان

        public int JobId { get; set; } // الوظيفة المرتبط بها الامتحان

        public string Title { get; set; } = null!; // عنوان الامتحان
    }
}
