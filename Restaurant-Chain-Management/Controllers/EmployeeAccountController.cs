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
    }
}
