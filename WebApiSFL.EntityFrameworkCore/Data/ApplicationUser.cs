using Microsoft.AspNetCore.Identity;

namespace WebApiSFL.EntityFrameworkCore.Data {
    public class ApplicationUser : IdentityUser {
        public string FullName { get; set; } = string.Empty;
    }
}
