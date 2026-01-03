using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReadyHire.Models.CompanyProfile
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string JobTitle { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string JobCategory { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string JobType { get; set; } = null!; // Full Time, Part Time, etc.

        [Required]
        [MaxLength(100)]
        public string ExperienceLevel { get; set; } = null!; // Entry, Intermediate, Senior

        [Required]
        public List<string> Skills { get; set; } = new List<string>(); // Multiple Skills

        [Required]
        public string JobDescription { get; set; } = null!;

        public DateTime? DeadlineForApplications { get; set; }

        public decimal? ExpectedSalary { get; set; }

        public bool IsSalaryNegotiable { get; set; } = false;

        [MaxLength(100)]
        public string? WorkingHours { get; set; }

        [MaxLength(255)]
        public string? JobLocation { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;




        [ForeignKey("CompanyProfileId")]
        public int CompanyProfileId { get; set; }

        
        public CompanyProfiles CompanyProfile { get; set; } = null!;

    }

}
