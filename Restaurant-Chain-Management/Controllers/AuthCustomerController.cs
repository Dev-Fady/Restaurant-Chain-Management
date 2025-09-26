using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Chain_Management.DTOs;
using Restaurant_Chain_Management.Models;
using Restaurant_Chain_Management.Services;
using System.Security.Claims;

namespace Restaurant_Chain_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthCustomerController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailService emailService;
        private readonly ITokenService tokenService;
        private readonly AppDbContext context;

        public AuthCustomerController(UserManager<ApplicationUser> userManager,
            IEmailService emailService,
            ITokenService tokenService,
            AppDbContext context)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.tokenService = tokenService;
            this.context = context;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterCustomerDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // 1- Create user in Identity
            var user = new ApplicationUser
            {
                UserName = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
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

            // 2- Send email confirmation link (optional)
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "EmployeeAccount",
                new { userId = user.Id, token = token }, Request.Scheme);

            var emailBody = EmailTemplateService.GetConfirmEmailTemplate(dto.Name, confirmationLink);
            await emailService.SendEmailAsync(user.Email, "Confirm your email", emailBody);

            // 5- Create Customer Profile and link it to the User
            var newCustomer = new Customer
            {
                Name = dto.Name,
                Address = dto.Address,
                BranchId = dto.BranchId,
                ApplicationUserId = user.Id,
                Phone = dto.PhoneNumber,
                Email = dto.Email

            };

            context.Customers.Add(newCustomer);
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
                    dto.BranchId,
                    dto.PhoneNumber,
                    dto.Address
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

        [Authorize]
        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized("Invalid Token");

            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            var customer = await context.Customers
                .Include(c=>c.Branch)
                .ThenInclude(b=>b.City)
                .Include(x=>x.Branch.Stock)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);

            return Ok(new
            {
                Success = true,
                Data = new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.PhoneNumber,
                    CustomerProfile = new
                    {
                        customer?.Id,
                        customer?.Name,
                        customer?.Address,
                        customer?.BranchId,
                        customer?.Phone,
                        customer?.Email,
                        BranchName= customer?.Branch.Name,
                        customer?.Branch.CityId,
                       CityName=  customer?.Branch.City.Name,
                       StockName=  customer?.Branch.Stock.Name,
                       StockID=  customer?.Branch.StockId,
                    }
                }
            });
        }

        [Authorize]
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized("Invalid Token");

            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            var result = await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            return Ok(new { Success = true, Message = "Password changed successfully ✅" });
        }

        [Authorize]
        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized("Invalid Token");

            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            // Delete Customer Profile
            var customer = await context.Customers.FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);
            if (customer != null)
            {
                context.Customers.Remove(customer);
                await context.SaveChangesAsync();
            }

            // Delete Identity User
            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            return Ok(new { Success = true, Message = "Account deleted successfully 🗑️" });
        }

    }
}
