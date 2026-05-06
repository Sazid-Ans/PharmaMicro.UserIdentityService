namespace PharmaMicro.UserIdentityService.Models
{
    public class PharmacyRegisterRequest
    {
        public string PharmacyName { get; set; } = string.Empty;
        public string PharmacyEmail { get; set; } = string.Empty;
        public string PharmacyPhoneNumber { get; set; } = string.Empty;
        public string AdminName { get; set; } = string.Empty;
        public string AdminPassword { get; set; } = string.Empty;
    }
}
