using System.ComponentModel.DataAnnotations;

namespace WebAPiSFl.Core.Entities.Register {
    public class Register {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]

        public string FullName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<string> Roles { get; set; }
    }
}
