using PharmaMicro.UserIdentityService.Models;

namespace PharmaMicro.UserIdentityService.Services.Interface
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}
