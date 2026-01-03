namespace ReadyHire.Models.Dto.UserProfileDto
{
    public class CvDto
    {

        public int Id { get; set; }

        // Foreign Key لربطه بـ UserProfile
        public int UserProfileId { get; set; }


        public string CvFilePath { get; set; }

    }
}
