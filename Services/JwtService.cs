using CuraMundi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CuraMundi.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _jwtkey;
        private readonly UserManager<User> _userManager;
        public JwtService(IConfiguration configuration)
        {
            _config = configuration;
            _jwtkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            //_userManager = userManager;
        }
        public async Task<string> GenerateJwtToken(User user, string role)
        {
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.Nom),
                new Claim(ClaimTypes.Surname, user.Prenom),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var credentials = new SigningCredentials(_jwtkey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(15),
                Issuer = _config["Jwt:Issuer"],
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
            //var token = new JwtSecurityToken(
            //    issuer: _config["Jwt:Issuer"],
            //    claims: claims,
            //    expires: DateTime.Now.AddMinutes(15),
            //    signingCredentials: credentials);

            ////string tempToken = new JwtSecurityTokenHandler().WriteToken(token);
            ////return tempToken;
            //return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
