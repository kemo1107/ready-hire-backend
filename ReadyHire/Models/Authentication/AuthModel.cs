namespace ReadyHire.Models.Authentication
{
    public class AuthModel
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public DateTime Expireson { get; set; }
        public string UserId { get; set; }

        public int? UserProfileId { get; set; }
        public int? CompanyProfileId { get; set; }

    }
}
