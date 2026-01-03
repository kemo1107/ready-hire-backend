public class UserProfileCreateDto
{
    public int? Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Location { get; set; }
    public string ApplicationUserId { get; set; }

    public string JobTitle { get; set; }
}