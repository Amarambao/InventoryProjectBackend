using BusinessLayer.Interfaces;
using CommonLayer.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLayer.Services
{
    public class JwtSrv : IJwtSrv
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUserEntity> _userManager;

        public JwtSrv(IConfiguration configuration, UserManager<AppUserEntity> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> GenerateJwtTokenAsync(AppUserEntity user)
        {
            var handler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JWT:Lifetime"])),
                Subject = await GenerateClaimsIdentityAsync(user),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            return handler.WriteToken(handler.CreateToken(tokenDescriptor));
        }

        private async Task<ClaimsIdentity> GenerateClaimsIdentityAsync(AppUserEntity user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Name, user.UserName!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

            var userRoles = await _userManager.GetRolesAsync(user);
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role.ToLower())));

            return new ClaimsIdentity(claims);
        }
    }
}
