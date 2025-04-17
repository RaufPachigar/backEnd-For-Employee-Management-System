using System.ComponentModel.DataAnnotations;

namespace WebAPiSFl.Core.Entities.Employee {
    public class Employee : BasicTableMaintanance {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Role { get; set; }
        public decimal Salary { get; set; }

        public string Department { get; set; }

        public string DateofJoining { get; set; }

    }
}
