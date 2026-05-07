using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PharmaMicro.UserIdentityService.Models;
using PharmaMicro.UserIdentityService.Models.Enums;
using PharmaMicro.UserIdentityService.Models.Users;
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
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthService> _logger;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ITokenService tokenService, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _logger = logger;
        }

        public Task AddClaimsAsync(string userId, IEnumerable<Claim> claims)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Authenticates a user by validating the provided email and password. It returns a JWT token and user details on success.
        /// The login request contain user email and password.
        /// The response contains token and email if successful.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AuthResponse> LoginAsync(UserLoginRequest request)
        {
            try
            {
                _logger.LogInformation("Login started for {request.Email}.", request.Email);
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                {
                    _logger.LogInformation("Email and password are required.");
                    return new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "Email and password are required."
                    };
                }

                //var user = _userManager.Users.Where(u => u.Email == request.Email).FirstOrDefault();
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    _logger.LogInformation("User not found, please register first.");
                    return new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "User not found, please register first."
                    };
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (!result.Succeeded)
                {
                    _logger.LogInformation("Invalid email or password.");
                    return new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid email or password."
                    };
                }

                var roles = await _userManager.GetRolesAsync(user);
                var token = _tokenService.GenerateJwtToken(user, roles.ToList());

                _logger.LogInformation("Login successful.");
                return new AuthResponse
                {
                    IsSuccess = true,
                    Message = "Login successful.",
                    Token = token,
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred while processing login.");
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "An error occurred while processing login.",
                };
            }
        }

        /// <summary>
        /// RegisterAsync first checks whether a user with the same email already exists.
        /// If the email is not already present, it generates a role-based user id.
        /// After successful user creation, it checks whether the requested role already exists in the role table, creates if needed and then adds the user to that role.
        /// Finally it returns true for success and false otherwise.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AuthResponse> RegisterAsync(UserRegisterRequest request)
        {
            try
            {
                _logger.LogInformation("Registration started for {request.Email}", request.Email);
                if (!_userManager.Users.Any(u => u.Email == request.Email))
                {
                    var userID = CreateUserID();
                    var user = new ApplicationUser
                    {
                        UserName = request.Email,
                        UserId = userID,
                        Email = request.Email,
                        FullName = request.FirstName + " " + request.LastName,
                    };
                    var result = await _userManager.CreateAsync(user, request.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "PAT");
                        _logger.LogInformation("Registration successful for {request.Email}.", request.Email);
                        return new AuthResponse
                        {
                            IsSuccess = true,
                            Message = "User registered successfully",
                            Token = "",
                        };
                    }
                    _logger.LogInformation("Registration failed for {request.Email}.", request.Email);
                    return new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "User registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }
                _logger.LogInformation("Registration failed because {request.Email} already exists.", request.Email);
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Email already exists"
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred while registering.");
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "An error occurred while registering."
                };
            }
        }

        /// <summary>
        /// Generates the next role specific user id using the appropriate prefix and sequential number.
        /// Returns an error message if the role is invalid.
        /// Input is the role used to determine the user id prefix and sequence.
        /// Returns a role specific user id.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        //private string CreateUserID(string role)
        //{
        //    string prefix;

        //    switch (role)
        //    {
        //        case nameof(Role.PHARMACIST):
        //            prefix = "PHARM";
        //            break;

        //        case nameof(Role.ADMIN):
        //            prefix = "ADM";
        //            break;

        //        case nameof(Role.MANAGER):
        //            prefix = "MNGR";
        //            break;

        //        case nameof(Role.PATIENT):
        //            prefix = "PAT";
        //            break;

        //        default:
        //            throw new ArgumentException("Invalid role");
        //    }
        //    int nextNumber = _userManager.Users.Count(u => u.UserId!=null && u.UserId.StartsWith(prefix)) + 1;
        //    return $"{prefix}{nextNumber}";
        //}
        private string CreateUserID()
        {
            string prefix = "PAT";

            int nextNumber = _userManager.Users.Count(u => u.UserId != null && u.UserId.StartsWith(prefix)) + 1;
            return $"{prefix}{nextNumber}";
        }
    }
}
