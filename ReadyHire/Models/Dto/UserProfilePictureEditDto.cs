using System.ComponentModel.DataAnnotations.Schema;

namespace ReadyHire.Models.Dto
{
    public class UserProfilePictureEditDto
    {

        [NotMapped]
        public IFormFile? Image { get; set; }



    }
}
