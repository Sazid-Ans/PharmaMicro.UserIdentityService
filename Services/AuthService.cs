using Microsoft.AspNetCore.Identity;
using PharmaMicro.UserIdentityService.Models;
using PharmaMicro.UserIdentityService.Models.Enums;
using PharmaMicro.UserIdentityService.Services.Interface;
using System.Security.Claims;

namespace PharmaMicro.UserIdentityService.Services
{
    public class AuthService : IAuthService
    {
        public UserManager<ApplicationUser> _userManager;
        public SignInManager<ApplicationUser> _signInManager;
        public RoleManager<IdentityRole> _roleManager;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public Task AddClaimsAsync(string userId, IEnumerable<Claim> claims)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            if(request.Email == null || request.Password == null) 
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Email and password are required"
                };
            }
            var user = _userManager.Users.Where(u => u.Email == request.Email).FirstOrDefault();
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
            return new AuthResponse
            {
                IsSuccess = true,
                Message = "Login successful",
                Token = "",
                Email = user.Email
            };
        }

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
            }
            return new AuthResponse
            {
                IsSuccess = false,
                Message = "User registration failed"
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
