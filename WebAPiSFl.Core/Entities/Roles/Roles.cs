using System.ComponentModel.DataAnnotations;

namespace WebAPiSFl.Core.Entities.Roles {
    public class Roles {
        [Required(ErrorMessage = "Role Name is required.")]
        public string RoleName { get; set; } = null!;
    }
}
