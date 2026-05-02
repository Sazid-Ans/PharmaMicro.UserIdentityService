using PharmaMicro.UserIdentityService.Models;

namespace PharmaMicro.UserIdentityService.Services
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}
