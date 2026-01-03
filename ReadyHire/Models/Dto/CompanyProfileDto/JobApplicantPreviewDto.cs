public class JobApplicantPreviewDto
{
    public int UserProfileId { get; set; }
    public string FullName { get; set; }
    public double MatchRatio { get; set; }
    public string CvFilePath { get; set; }
    public string ProfilePictureUrl { get; set; }
    public DateTime AppliedAt { get; set; }
}
