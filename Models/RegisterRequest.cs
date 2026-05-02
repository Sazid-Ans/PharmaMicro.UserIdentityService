namespace PharmaMicro.UserIdentityService.Models
{
    /// <summary>
    /// Request class for registration input.
    /// </summary>
    public class RegisterRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty ;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "Pharmacist";
    }
}
