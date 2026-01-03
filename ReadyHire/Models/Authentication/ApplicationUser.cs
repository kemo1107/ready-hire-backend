using ReadyHire.Models.UserProfile;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReadyHire.Models.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        // 🟡 Nullable علشان مش كل المستخدمين عندهم UserProfile مباشرة
        [ForeignKey("UserProfileId")]
        public int? UserProfileId { get; set; }

        public UserProfiles? UserProfiles { get; set; }

        // 🟡 Nullable علشان مش كل المستخدمين عندهم شركة
        [ForeignKey("CompanyProfilesId")]
        public int? CompanyProfilesId { get; set; }

        public CompanyProfiles? CompanyProfiles { get; set; }
    }
}
