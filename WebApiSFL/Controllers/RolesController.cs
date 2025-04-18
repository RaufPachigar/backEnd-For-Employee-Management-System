﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiSFL.EntityFrameworkCore.Data;
using WebAPiSFl.Core.Entities.Roles;

namespace WebApiSFL.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] Roles Role) {
            if (string.IsNullOrEmpty(Role.RoleName)) {
                return BadRequest("Role name is required");
            }

            var roleExist = await _roleManager.RoleExistsAsync(Role.RoleName);

            if (roleExist) {
                return BadRequest("Role already exist");
            }

            var roleResult = await _roleManager.CreateAsync(new IdentityRole(Role.RoleName));

            if (roleResult.Succeeded) {
                return Ok(new { message = "Role Created successfully" });
            }

            return BadRequest("Role creation failed.");

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleResponse>>> GetRoles() {


            // list of roles with total users in each role 

            var roles = await _roleManager.Roles.Select(r => new RoleResponse {
                Id = r.Id,
                Name = r.Name,
                TotalUsers = _userManager.GetUsersInRoleAsync(r.Name!).Result.Count
            }).ToListAsync();

            return Ok(roles);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id) {
            // find role by their id

            var role = await _roleManager.FindByIdAsync(id);

            if (role is null) {
                return NotFound("Role not found.");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded) {
                return Ok(new { message = "Role deleted successfully." });
            }

            return BadRequest("Role deletion failed.");

        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRole([FromBody] RoleAssign roleAssign) {
            var user = await _userManager.FindByIdAsync(roleAssign.UserId);

            if (user is null) {
                return NotFound("User not found.");
            }

            var role = await _roleManager.FindByIdAsync(roleAssign.RoleId);

            if (role is null) {
                return NotFound("Role not found.");
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name!);

            if (result.Succeeded) {
                return Ok(new { message = "Role assigned successfully" });
            }

            var error = result.Errors.FirstOrDefault();

            return BadRequest(error!.Description);

        }
    }
}
