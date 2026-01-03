using ReadyHire.Models.Dto;
using ReadyHire.Models.Dto.UserProfileDto;

public class UserProfileDto
{
    public int? Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Location { get; set; }

    public string JobTitle { get; set; } 
    public string ApplicationUserId { get; set; }
    public int CvId { get; set; }
    public CvDto Cv { get; set; }

    public int? UserOverViewId { get; set; }
    public UserOverViewDto UserOverView { get; set; }

    public List<EducationDto> Educations { get; set; } = new();
    public List<ExperienceDto> Experiences { get; set; } = new();
    public List<SkillsDto> Skills { get; set; } = new();
    public UserProfilePictureDto? UserProfilePicture { get; set; }

}
