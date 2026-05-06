namespace PharmaMicro.UserIdentityService.Models
{
    public class Pharmacy
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PharmacyCode { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
