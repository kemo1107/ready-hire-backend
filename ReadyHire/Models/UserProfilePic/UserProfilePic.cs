using ReadyHire.Models.Authentication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReadyHire.Models.UserProfilePic
{
    public class UserProfilePic
    {
        [Key]
        public int UserProfilePictureId { get; set; }

        public string UserId { get; set; } = null!;

        // بدل ما نخزن الصورة كـ byte[]، نخزن الرابط فقط
        public string Image { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
