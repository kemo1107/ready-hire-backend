namespace ReadyHire.Models.Dto.CompanyProfileDto
{
    public class JobDto
    {
        public int Id { get; set; } 

        public string JobTitle { get; set; } = null!; 
        public string JobCategory { get; set; } = null!; 

        public string JobType { get; set; } = null!; 

        public string ExperienceLevel { get; set; } = null!; 

        public List<string> Skills { get; set; } = new(); 

        public string JobDescription { get; set; } = null!; 

        public DateTime? DeadlineForApplications { get; set; } 
        public decimal? ExpectedSalary { get; set; } 

        public bool IsSalaryNegotiable { get; set; } 

        public string? WorkingHours { get; set; } 

        public string? JobLocation { get; set; } 

        public DateTime CreatedAt { get; set; } 
        public int CompanyProfileId { get; set; } 

        public string? CompanyImageUrl { get; set; }
        public string? CompanyName { get; set; }

        public string? CompanyLocation { get; set; }


    }
}
