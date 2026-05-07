using PharmaMicro.UserIdentityService.Models.Enums;
using PharmaMicro.UserIdentityService.Models.Users;

namespace PharmaMicro.UserIdentityService.Models.Pharmacy
{
    public class PharmacyStaff
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public Guid PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }

        public PharmacyRole Role { get; set; } 
        public StaffCreationStatus Status { get; set; } 
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string ApprovedBy { get; set; }
    }

}
