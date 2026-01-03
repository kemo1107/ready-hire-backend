using System.ComponentModel.DataAnnotations;

namespace ReadyHire.Models.Authentication
{
    public class mailrequest
    {
        [Required]

        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }
}
