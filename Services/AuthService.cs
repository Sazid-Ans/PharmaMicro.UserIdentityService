using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PharmaMicro.UserIdentityService.Models;
using PharmaMicro.UserIdentityService.Models.Enums;
using PharmaMicro.UserIdentityService.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PharmaMicro.UserIdentityService.Services
{
    public class AuthService : IAuthService
    {
        public UserManager<ApplicationUser> _userManager;
        public SignInManager<ApplicationUser> _signInManager;
        public RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public Task AddClaimsAsync(string userId, IEnumerable<Claim> claims)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            if(string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password)) 
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Email and password are required"
                };
            }

            //var user = _userManager.Users.Where(u => u.Email == request.Email).FirstOrDefault();
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "User not found, please register first"
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles.ToList());

            return new AuthResponse
            {
                IsSuccess = true,
                Message = "Login successful",
                Token = token,
                Email = user.Email!,
            };
        }

        private string GenerateJwtToken(ApplicationUser user, List<string> list)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim("UserId", user.UserId ?? string.Empty),
                new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            // Define key and signing credentials
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Generate token with issuer, audience, and expiration
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// RegisterAsync first checks whether a user with the same email already exists.
        /// If the email is not already present, it generates a role-based user id.
        /// After successful user creation, it checks whether the requested role already exists in the role table, creates if needed and then adds the user to that role.
        /// Finally it returns true for success and false otherwise.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if(!_userManager.Users.Any(u => u.Email == request.Email))
            {
                var userID = CreateUserID(request.Role);
                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    UserId = userID,
                    Email = request.Email,
                    FullName = request.FullName
                };
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.Roles.Any(r => r.Name == request.Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(request.Role));
                    }
                    await _userManager.AddToRoleAsync(user, request.Role);
                    return new AuthResponse
                    {
                        IsSuccess = true,
                        Message = "User registered successfully",
                        Token = "",
                        Email = user.Email
                    };
                }
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "User registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }
            return new AuthResponse
            {
                IsSuccess = false,
                Message = "Email already exists"
            };
        }

        private string CreateUserID(string role)
        {
            string prefix;

            switch (role)
            {
                case nameof(Role.Pharmacist):
                    prefix = "PHARM";
                    break;

                case nameof(Role.Admin):
                    prefix = "ADM";
                    break;

                case nameof(Role.Manager):
                    prefix = "MNGR";
                    break;

                case nameof(Role.Patient):
                    prefix = "PAT";
                    break;

                default:
                    throw new ArgumentException("Invalid role");
            }
            int nextNumber = _userManager.Users.Count() + 1;
            return $"{prefix}{nextNumber}";
        }
    }
}
