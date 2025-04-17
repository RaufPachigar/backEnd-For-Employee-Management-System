using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiSFL.EntityFrameworkCore.Data;
using WebAPiSFl.Core.Entities.AuthResponse;
using WebAPiSFl.Core.Entities.Login;
using WebAPiSFl.Core.Entities.Register;
using WebAPiSFl.Core.Entities.UserDetails;

namespace WebApiSFL.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase {
        private readonly UserManager<ApplicationUser> _userManager;


        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager, IConfiguration configuration) {


            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<string>> Register(Register register) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var user = new ApplicationUser {
                Email = register.Email,
                FullName = register.FullName,
                UserName = register.Email,


            };
            var result = await _userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded) {
                return BadRequest(result.Errors);
            }



            // Automatically assign the "User" role to every new user
            if (!await _roleManager.RoleExistsAsync("User")) {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }
            await _userManager.AddToRoleAsync(user, "User");


            return Ok(new AuthResponse {
                IsSuccess = true,
                Message = "Account Created Sucessfully!"
            });




        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponse>> Login(Login login) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user == null) {
                return Unauthorized(new AuthResponse {
                    IsSuccess = false,
                    Message = "user not found with this email"
                });
            }
            var result = await _userManager.CheckPasswordAsync(user, login.Password);

            if (!result) {
                return Unauthorized(new AuthResponse {
                    IsSuccess = false,
                    Message = "invalid password"
                });

            }
            var token = GenerateToken(user);

            return Ok(new AuthResponse {
                Token = token,
                IsSuccess = true,
                Message = "Login Success."
            });
        }
        [AllowAnonymous]
        [HttpPost("CreateAdmin")]
        public async Task<ActionResult<string>> CreateAdmin(Register register) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser {
                Email = register.Email,
                FullName = register.FullName,
                UserName = register.Email,
            };

            var result = await _userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded) {
                return BadRequest(result.Errors);
            }

            // Assign the "Admin" role to the user
            if (!await _roleManager.RoleExistsAsync("Admin")) {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            await _userManager.AddToRoleAsync(user, "Admin");

            return Ok(new AuthResponse {
                IsSuccess = true,
                Message = "Admin account created successfully!"
            });
        }

        private string GenerateToken(ApplicationUser user) {

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII
            .GetBytes(_configuration.GetSection("JWTSetting").GetSection("securityKey").Value!);

            var roles = _userManager.GetRolesAsync(user).Result;

            List<Claim> claims =
            [
                new (JwtRegisteredClaimNames.Email, user.Email??""),
                new (JwtRegisteredClaimNames.Name, user. FullName ?? ""),
                new (JwtRegisteredClaimNames.NameId, user.Id ??""),
                new (JwtRegisteredClaimNames.Aud,
                     _configuration.GetSection("JWTSetting").GetSection("ValidAudience").Value!),
                new (JwtRegisteredClaimNames.Iss, _configuration.GetSection("JWTSetting").GetSection("ValidIssuer").Value!)
            ];

            foreach (var role in roles) {

                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor {

                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256

            )


            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);


        }

        //api/account/detail
        [HttpGet("detail")]
        public async Task<ActionResult<UserDetails>> GetUserDetail() {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(currentUserId!);


            if (user is null) {
                return NotFound(new AuthResponse {
                    IsSuccess = false,
                    Message = "User not found"
                });
            }

            return Ok(new UserDetails {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = [.. await _userManager.GetRolesAsync(user)],
                //PhoneNumber = user.PhoneNumber,
                //PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                //AccessFailedCount = user.AccessFailedCount,

            });

        }


        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<UserDetails>>> GetUsers() {
        //    var users = await _userManager.Users.Select(u => new UserDetails {
        //        Id = u.Id,
        //        Email = u.Email,
        //        FullName = u.FullName,
        //        Roles = _userManager.GetRolesAsync(u).Result.ToArray()
        //    }).ToListAsync();

        //    return Ok(users);
        //}

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers() {
            var users = _userManager.Users.ToList();
            var result = new List<UserDetails>();

            foreach (var user in users) {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new UserDetails {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id) {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded) {
                return Ok(new AuthResponse {
                    IsSuccess = true,
                    Message = "Deleted successfully"
                });
            }

            return BadRequest(result.Errors);
        }

    }
}

