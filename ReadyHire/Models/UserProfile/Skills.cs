using ReadyHire.Models.UserProfile;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Skills
{

    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [ForeignKey("UserProfileId")]
    public int UserProfileId { get; set; }

    public UserProfiles UserProfile { get; set; }




}