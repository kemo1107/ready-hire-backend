using ReadyHire.Models.UserProfile;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Experience
{
    public int Id { get; set; }

    [Required]
    public string JobTitle { get; set; }

    [Required]
    public string OrganizationName { get; set; }

    [Required]
    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }


    [ForeignKey("UserProfileId")]
    public int UserProfileId { get; set; }

    public UserProfiles UserProfile { get; set; }
}