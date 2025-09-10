using CuraMundi.Domain.Entities;
using CuraMundi.Dto;
using CuraMundi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace CuraMundi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly JwtService _jwtService;

        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager, JwtService jwtService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
            {
                return BadRequest("Invalid credentials");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result.Succeeded)
            {
                return Accepted(_jwtService.GenerateJwtToken(user,role));
            }
            return BadRequest("Invalid credentials");
        }
        [HttpGet("email-confirm")]
        public async Task<ActionResult<string>> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return BadRequest("Invalid user");
            }
            if (user.EmailConfirmed)
            {
                return BadRequest("Email already confirmed");
            }
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Accepted("Email confirmed");
            }
            return BadRequest("Invalid token");
        }
    }
}
