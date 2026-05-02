namespace PharmaMicro.UserIdentityService.Models
{
    /// <summary>
    /// Request class for login input.
    /// </summary>
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
