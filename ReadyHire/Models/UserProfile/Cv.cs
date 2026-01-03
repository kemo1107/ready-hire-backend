using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ReadyHire.Models.UserProfile;

public class Cv
{
    [Key]
    public int Id { get; set; }

    public string CvFilePath { get; set; } // أو CvUrl حسب انت هتخزنه ازاي

    [ForeignKey("UserProfileId")]
    public int UserProfileId { get; set; }

    public UserProfiles UserProfile { get; set; }
}