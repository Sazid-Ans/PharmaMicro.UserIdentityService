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
        public string UserId { get; set; } = string.Empty;
        public virtual Pharmacy Pharmacy { get; set; }
        /*public Guid PharmacyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn {  get; set; }
        public DateTime? LastLogin { get;set; }*/
    }
}
