using Microsoft.AspNetCore.Identity;
using PharmaMicro.UserIdentityService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PharmaMicro.UserIdentityService.Services.Interface;

namespace PharmaMicro.UserIdentityService.Services
{
    /// <summary>
    /// This service generates the JWT Token after Identity cinfirms the user is valid.
    /// JWT bearer authentication in ASP.NET Core validates that token on later requests and extracts the user identity from its claims.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        public TokenService(IConfiguration configuration, UserManager<ApplicationUser> userManager) 
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FullName ?? user.UserName ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? user.Email ?? ""),
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: _configuration["Jwt:Issuer"], audience: _configuration["Jwt:Audience"],
                claims:claims, expires:DateTime.UtcNow.AddHours(1),
                signingCredentials:credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
