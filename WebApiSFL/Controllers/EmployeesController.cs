using Microsoft.AspNetCore.Mvc;
using WebApiSFl.Service.EmployeeService;
using WebAPiSFl.Core.Entities.Employee;

namespace WebApiSFL.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService) {
            _employeeService = employeeService;
        }
        [HttpGet("GetData")]
        public async Task<ActionResult> GetDataAllAsync() {
            var std = await _employeeService.GetDataAsync();
            return Ok(std);
        }

        [HttpGet("GetData/{id}")]
        public async Task<ActionResult> GetDataById(int id) {
            var std = await _employeeService.GetDataByIntAsync(id);
            return Ok(std);
        }

        [HttpPost("CreateData")]
        public async Task<ActionResult> CreateData(EmployeeDTO employeeDTO) {
            //if (!ModelState.IsValid) {
            //    return BadRequest(ModelState);
            //}
            await _employeeService.CreateDataAsync(employeeDTO);
            return Ok();
        }

        [HttpPut("UpdateData")]
        public async Task<ActionResult> UpdateData(Employee employee) {

            var result = await _employeeService.UpdateDataAsync(employee);
            return Ok(new { message = "Updated successfully" });

        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteData(int id) {
            var delete = await _employeeService.DeleteDataAsync(id);
            if (delete == null) {
                return NotFound(new { message = "Employee not found" });
            }
            return Ok(new { message = "Deleted successfully" });
        }


    }
}



