using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPiSFl.Core.Entities.Employee;

namespace WebApiSFL.EntityFrameworkCore.Data {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext(DbContextOptions options) : base(options) {
        }



        public DbSet<Employee> Employees { get; set; }


    }
}
