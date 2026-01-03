using ReadyHire.Models.UserProfile;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserLanguage
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string LanguageType { get; set; }

    [ForeignKey("UserProfileId")]

    public int UserProfileId { get; set; }

    //[ForeignKey(nameof(UserProfileId))]
    public UserProfiles UserProfile { get; set; }
}