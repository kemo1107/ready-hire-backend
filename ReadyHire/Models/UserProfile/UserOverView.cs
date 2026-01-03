using ReadyHire.Models.UserProfile;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserOverView
{
    [Key]
    public int Id { get; set; }
    [Required]

    public string Disciption { get; set; }

    public  string Title { get; set; } 

    [ForeignKey("UserProfileId")]
    
    public int UserProfileId { get; set; }
    public UserProfiles Profile { get; set; }
}