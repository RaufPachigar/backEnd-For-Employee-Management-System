using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPiSFl.Core.Entities;
using WebAPiSFl.Core.Entities.Employee;

namespace WebApiSFl.Service.EmployeeService
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetDataAsync();

        Task<Employee> GetDataByIntAsync(int id);
        Task CreateDataAsync(EmployeeDTO employeeDTO);
        Task<string> UpdateDataAsync(Employee employee);
        Task<string> DeleteDataAsync(int id);
    }
}
