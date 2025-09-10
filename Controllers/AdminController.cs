using CuraMundi.Domain.Entities;
using CuraMundi.Dto;
using CuraMundi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CuraMundi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public AdminController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserCreateDto userCreateDto)
        {
            User user = new User
            {
                Nom = userCreateDto.Nom.ToUpperCase(),
                Prenom = userCreateDto.Prenom.Capitalize(),
                Email = userCreateDto.Email,
                UserName = userCreateDto.Email,
                EmailConfirmed = true,

            };
            var usr = await _userManager.CreateAsync(user, userCreateDto.Password);
            if (usr.Succeeded)
            {
                var role = await _userManager.AddToRoleAsync(user, userCreateDto.Role);
                return role.Succeeded ? Ok(user) : BadRequest(usr.Errors);



            }
            return BadRequest(usr.Errors);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersDto = new List<UserDetailDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Count > 0)
                {
                    usersDto.Add(new UserDetailDto
                    {
                        Id = user.Id,
                        Nom = user.Nom,
                        Prenom = user.Prenom,
                        Email = user.Email,
                        Role = roles[0],
                        Active = user.EmailConfirmed
                    }); 
                    
                }
                //usersDto.Add(user.ToUserDetailDto());
            }
            return Ok(usersDto.OrderBy(u=>u.Nom));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            User user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(Guid id, [FromBody] UserCreateDto userCreateDto)
        {
            User user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                return NotFound();
            }
            user.Nom = userCreateDto.Nom.ToUpperCase() ?? user.Nom;
            user.Prenom = userCreateDto.Prenom.Capitalize() ?? user.Prenom;
            user.Email = userCreateDto.Email ?? user.Email;

            var usr = await _userManager.UpdateAsync(user);
            if (usr.Succeeded)
            {
                return Ok(user);
            }
            return BadRequest(usr.Errors);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            User user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                return NotFound();
            }
            var usr = await _userManager.DeleteAsync(user);
            if (usr.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(usr.Errors);
        }

    }
}
