using PharmaMicro.UserIdentityService.Models;
using PharmaMicro.UserIdentityService.Models.Users;
using System.Security.Claims;

namespace PharmaMicro.UserIdentityService.Services.Interface
{
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user with the provided registration details.
        /// </summary>
        /// <param name="request">The registration request containing user details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an AuthResponse   indicating success or failure.</returns>
       Task<AuthResponse> RegisterAsync(UserRegisterRequest request);
        /// <summary>
        /// Authenticates a user with the provided login credentials.
        /// </summary>
        /// <param name="request">The login request containing user credentials.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an AuthResponse indicating success or failure.</returns>
        Task<AuthResponse> LoginAsync(UserLoginRequest request);
        //Summary:
        ///Add claims to the user identity, which can be used for authorization and access control in the application. Claims are key-value pairs that represent user attributes or permissions.
        /// <param name="userId">The unique identifier of the user to whom the claims will be added.</param>
        /// <param name="claims">The claims to be added to the user's identity.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddClaimsAsync(string userId, IEnumerable<Claim> claims);

    }
}
