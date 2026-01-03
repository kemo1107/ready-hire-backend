using System.ComponentModel.DataAnnotations;

namespace ReadyHire.Models.Authentication
{
    public class tokenrequest
    {
        [Required]
        public string email { get; set; }
    }
}
