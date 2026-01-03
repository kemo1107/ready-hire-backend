using ReadyHire.Models.Authentication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReadyHire.Models.UserProfile
{
    public class UserProfiles
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Location { get; set; }

        public string JobTitle { get; set; }


        [ForeignKey("ApplicationUserId")]
       public string  ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }


        [ForeignKey("CvId")]
        public int CvId { get; set; }

        public  Cv cv { get; set; }

        public ICollection<Experience> Experiences { get; set; } = new List<Experience>();
        public ICollection<Education> Educations { get; set; } = new List<Education>();
        public ICollection<Skills> Skills { get; set; } = new List<Skills>();

        public ICollection<UserLanguage> Languages { get; set; } = new List<UserLanguage>();

        [ForeignKey("UserOverViewId")]
        public int? UserOverViewId { get; set; }
       public UserOverView? UserOverView { get; set; }




     

    

    }
}
