using Microsoft.EntityFrameworkCore;
using WebApiSFL.EntityFrameworkCore.Data;
using WebAPiSFl.Core.Entities.Employee;

namespace WebApiSFl.Service.EmployeeService {
    public class EmployeeService : IEmployeeService {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeService(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Employee>> GetDataAsync() {
            return await _dbContext.Employees.ToListAsync();

        }

        public async Task<Employee> GetDataByIntAsync(int id) {
            return await _dbContext.Employees.FindAsync(id);
        }

        public async Task CreateDataAsync(EmployeeDTO employeeDTO) {

            var employee = new Employee {
                Name = employeeDTO.Name,
                Role = employeeDTO.Role,
                Salary = employeeDTO.Salary,
                Department = employeeDTO.Department,
                DateofJoining = employeeDTO.DateofJoining,
            };


            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

        }

        public async Task<string> UpdateDataAsync(Employee employee) {
            var existingStudent = await GetDataByIntAsync(employee.Id);
            if (existingStudent != null) {
                existingStudent.Id = employee.Id;
                existingStudent.Name = employee.Name;
                existingStudent.Role = employee.Role;
                existingStudent.Salary = employee.Salary;
                existingStudent.Department = employee.Department;
                existingStudent.DateofJoining = employee.DateofJoining;

                _dbContext.Employees.Update(existingStudent);
                await _dbContext.SaveChangesAsync();
                return "Updated successfully";
            }
            return "Employee not found";
        }

        public async Task<string> DeleteDataAsync(int id) {
            var student = await _dbContext.Employees.FindAsync(id);
            if (student != null) {
                _dbContext.Employees.Remove(student);
                await _dbContext.SaveChangesAsync();
                return "Deleted successfully";
            }
            return "Employee not found";
        }


    }
}
