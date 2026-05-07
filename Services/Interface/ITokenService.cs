using PharmaMicro.UserIdentityService.Models.Users;

namespace PharmaMicro.UserIdentityService.Services.Interface
{
    public interface ITokenService
    {
        //Task<string> GenerateTokenAsync(ApplicationUser user);
        public string GenerateJwtToken(ApplicationUser user, List<string> list);
    }
}
