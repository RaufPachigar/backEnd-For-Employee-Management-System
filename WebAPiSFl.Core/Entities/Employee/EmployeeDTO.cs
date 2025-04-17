using System.ComponentModel.DataAnnotations;

namespace WebAPiSFl.Core.Entities.Employee {
    public class EmployeeDTO {
        [Required]
        public string Name { get; set; }
        public string Role { get; set; }
        public decimal Salary { get; set; }
        public string Department { get; set; }

    }
}
