using ReadyHire.Models.Dto;

public class UserProfilePictureAddDto : UserProfilePictureEditDto
{
    public string UserId { get; set; }

    public IFormFile Image { get; set; }

}