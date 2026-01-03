using ReadyHire.Models.Authentication;
using ReadyHire.Models.CompanyProfile;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CompanyProfiles
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string CompanyName { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string ResponsiblePersonJobTitle { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string Industry { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string OfficialContactMethods { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string Location { get; set; } = null!;

    [Required]
    public DateTime YearEstablished { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Job> jobs { get; set; }= new List<Job>();

    [ForeignKey("ApplicationUserId")]
    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}
