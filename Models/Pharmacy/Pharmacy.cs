namespace PharmaMicro.UserIdentityService.Models.Pharmacy
{
    public class Pharmacy
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LicenseNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PharmacyStaff> StaffMembers { get; set; }
    }

}
