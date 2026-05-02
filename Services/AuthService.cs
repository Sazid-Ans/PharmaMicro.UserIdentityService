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

        public Task<string?> LoginAsync(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegisterAsync(RegisterRequest request)
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
                var result = _userManager.CreateAsync(user, request.Password).Result;
                if (result.Succeeded)
                {
                    if (!_roleManager.Roles.Any(r => r.Name == request.Role))
                    {
                        _roleManager.CreateAsync(new IdentityRole(request.Role)).Wait();
                    }
                    _userManager.AddToRoleAsync(user, request.Role).Wait();
                    return Task.FromResult(true);
                }
            }
            return Task.FromResult(false);
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
