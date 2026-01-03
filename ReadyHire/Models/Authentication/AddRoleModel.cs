using System.ComponentModel.DataAnnotations;

namespace ReadyHire.Models.Authentication
{
    public class AddRoleModel
    {
        [Required]
        public string userid { get; set; }

        [Required]
        public string role { get; set; }
    }
}
