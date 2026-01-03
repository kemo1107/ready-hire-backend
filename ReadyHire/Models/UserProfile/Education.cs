using ReadyHire.Models.UserProfile;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Education
{

    public int Id { get; set; }

    [Required]
    public string University { get; set; }

    public string Faculty { get; set; }

    public string Degree { get; set; }

    [Required]
    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }


    [ForeignKey("UserProfileId")]
    public int UserProfileId { get; set; }

    public UserProfiles UserProfile { get; set; }
}
