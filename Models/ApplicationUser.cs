using Microsoft.AspNetCore.Identity;

namespace PharmaMicro.UserIdentityService.Models
{
    /// <summary>
    /// IdentityUser already has the standard fields. 
    /// By inheriting it, we can reuse Microsoft's identity system and add any extra project specific fields.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
