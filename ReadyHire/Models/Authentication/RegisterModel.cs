using System.ComponentModel.DataAnnotations;

namespace ReadyHire.Models.Authentication
{
    public class RegisterModel
    {
        [Required, StringLength(30)]
        public string FirstName { get; set; }

        [Required, StringLength(30)]
        public string LastName { get; set; }

        [Required, StringLength(70)]
        public string UserName { get; set; }

        [Required, StringLength(150)]
        public string Email { get; set; }

        [Required, StringLength(100)]
        public string Password { get; set; }
    }
}
