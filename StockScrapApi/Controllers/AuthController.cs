using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockScrapApi.Data;
using StockScrapApi.Dtos;
using StockScrapApi.Models;
using StockScrapApi.Services;

namespace StockScrapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<ApiUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AuthController(IAuthManager authManager, UserManager<ApiUser> userManager, ILogger<AuthController> logger, ApplicationDbContext context)
        {
            _authManager = authManager;
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            _logger.LogInformation("Login Attemp for {0}", loginDto.UserName);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isValidCredential = await _authManager.ValidateUser(loginDto);

            if (!isValidCredential)
            {
                return Unauthorized();
            }

            return Accepted(new { Token = await _authManager.CreateToken() });
        }

        [Authorize(Roles = "Admin,SuperUser")]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            var roles = await _context.Roles.Select(x => x.Name).ToListAsync();

            foreach(var role in registerUserDto.Roles)
            {
                if (!roles.Contains(role))
                {
                    return BadRequest("One or more invalid role(s).");
                }    
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new ApiUser
            {
                Email = registerUserDto.Email,
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                UserName = registerUserDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerUserDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            try
            {
                await _userManager.AddToRolesAsync(user, registerUserDto.Roles);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occured assigning role.");
                await _userManager.DeleteAsync(user);
                return StatusCode(500, "Internal Server Error. Please Try again later.");
            }

            return Accepted();
        }
    
    }
}
