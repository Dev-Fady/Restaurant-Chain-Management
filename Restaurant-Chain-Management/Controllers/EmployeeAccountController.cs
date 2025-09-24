using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Chain_Management.DTOs;
using Restaurant_Chain_Management.Models;
using Restaurant_Chain_Management.Services;

namespace Restaurant_Chain_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailService emailService;
        private readonly ITokenService tokenService;
        private readonly AppDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;

        public EmployeeAccountController(
            UserManager<ApplicationUser> userManager, 
            IEmailService emailService, 
            ITokenService tokenService,
            AppDbContext context,
                RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.tokenService = tokenService;
            this.context = context;
            this.roleManager = roleManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterEmployeeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // 1- Create user in Identity
            var user = new ApplicationUser
            {
                UserName = dto.Name,
                Email = dto.Email,
            };

            var result = await userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("Data", item.Description);
                }
                return BadRequest(ModelState);
            }

            // 2- Ensure the Role exists in Identity Roles
            var roleName = dto.Role.ToString(); // "GeneralManager", "BranchManager", or "Staff"
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // 3- Assign the Role to the User
            await userManager.AddToRoleAsync(user, roleName);

            // 4- Send email confirmation link (optional)
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "EmployeeAccount",
                new { userId = user.Id, token = token }, Request.Scheme);

            var emailBody = EmailTemplateService.GetConfirmEmailTemplate(dto.Name, confirmationLink);
            await emailService.SendEmailAsync(user.Email, "Confirm your email", emailBody);

            // 5- Create Employee Profile and link it to the User
            var employee = new Employee
            {
                Name = dto.Name,
                Address = dto.Address,
                Role = dto.Role,
                BranchId = dto.BranchId,
                ApplicationUserId = user.Id,
                Salary = dto.Salary
            };

            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            // 6- Return success response
            return Ok(new
            {
                Success = true,
                Message = "Employee registered successfully! Please check your email to confirm your account.",
                Data = new
                {
                    dto.Name,
                    dto.Email,
                    dto.Role,
                    dto.BranchId
                }
            });
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("Invalid Email Confirmation Request");

            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return Ok(new { Success = true, Message = "✅ Email confirmed successfully. You can now log in." });

            return BadRequest(new { Success = false, Message = "❌ Email confirmation failed. Please try again." });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto UserFromRequset)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userFromDB = await userManager.FindByEmailAsync(UserFromRequset.Email);
            if (userFromDB == null) return BadRequest("User not found");

            if (!await userManager.IsEmailConfirmedAsync(userFromDB))
                return BadRequest("Please confirm your email before logging in.");

            bool found = await userManager.CheckPasswordAsync(userFromDB, UserFromRequset.Password);
            if (!found) return BadRequest("UserName or Password Wrong");

            var roles = await userManager.GetRolesAsync(userFromDB);
            var token = tokenService.CreateToken(userFromDB, roles);

            return Ok(new
            {
                token,
                expiration = DateTime.Now.AddHours(1)
            });
        }

        [Authorize]
        [HttpDelete("Logout")]
        public async Task<IActionResult> Logout()
        {
            // Invalidate the token on the client side by simply deleting it.
            // If you want to implement server-side token invalidation, consider using a token blacklist.
            return Ok(new { Success = true, Message = "Logged out successfully." });
        }

        [Authorize(Roles = "GeneralManager")]
        [HttpPut("UpdateEmployee/{id}")]
        public async Task<IActionResult> UpdateEmployee(int id,[FromForm] UpdateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = await context.Employees
                .Include(e => e.ApplicationUser)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return NotFound(new { Success = false, Message = "Employee not found." });

            // Update Employee details only if provided
            employee.Name = string.IsNullOrEmpty(dto.Name) ? employee.Name : dto.Name;
            employee.Address = string.IsNullOrEmpty(dto.Address) ? employee.Address : dto.Address;
            employee.Role = dto.Role == 0 ? employee.Role : dto.Role; // assuming Role = int/enum
            employee.BranchId = dto.BranchId ?? employee.BranchId;
            employee.Salary = dto.Salary ?? employee.Salary;

            // Update associated ApplicationUser details
            if (employee.ApplicationUser != null)
            {
                var user = await userManager.FindByIdAsync(employee.ApplicationUser.Id);
                if (user != null)
                {
                    user.UserName = string.IsNullOrEmpty(dto.Name) ? user.UserName : dto.Name;
                    user.Email = string.IsNullOrEmpty(dto.Email) ? user.Email : dto.Email;

                    var userUpdateResult = await userManager.UpdateAsync(user);
                    if (!userUpdateResult.Succeeded)
                    {
                        foreach (var error in userUpdateResult.Errors)
                        {
                            ModelState.AddModelError("UserUpdate", error.Description);
                        }
                        return BadRequest(ModelState);
                    }

                    // Update roles if changed and Role was provided
                    if (dto.Role != 0) // 0 = not provided (assuming int/enum)
                    {
                        var currentRoles = await userManager.GetRolesAsync(user);
                        var newRoleName = dto.Role.ToString();

                        if (!currentRoles.Contains(newRoleName))
                        {
                            await userManager.RemoveFromRolesAsync(user, currentRoles);
                            if (!await roleManager.RoleExistsAsync(newRoleName))
                            {
                                await roleManager.CreateAsync(new IdentityRole(newRoleName));
                            }
                            await userManager.AddToRoleAsync(user, newRoleName);
                        }
                    }
                }
            }

            await context.SaveChangesAsync();
            return Ok(new { Success = true, Message = "Employee updated successfully." });
        }


        [Authorize]
        [HttpDelete("DeleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var employee = await context.Employees
                              .Include(e => e.ApplicationUser)
                              .FirstOrDefaultAsync(e => e.Id == id);

                if (employee == null)
                    return NotFound(new { Success = false, Message = "Employee not found." });
                // Remove the associated ApplicationUser
                if (employee.ApplicationUser != null)
                {
                    var user = await userManager.FindByIdAsync(employee.ApplicationUser.Id);
                    if (user != null)
                    {
                        var result = await userManager.DeleteAsync(user);
                        if (!result.Succeeded)
                        {
                            await transaction.RollbackAsync();
                            return BadRequest(new { Success = false, Message = "Failed to delete associated user." });
                        }
                    }
                }
                // Remove the Employee record
                context.Employees.Remove(employee);
                await context.SaveChangesAsync();
                // Commit transaction
                await transaction.CommitAsync();
                return Ok(new { Success = true, Message = "Employee deleted successfully." });

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Success = false, Message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
