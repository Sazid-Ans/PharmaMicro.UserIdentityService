namespace PharmaMicro.UserIdentityService.Models.Users
{
    /// <summary>
    /// Request class for login input.
    /// </summary>
    public class UserLoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
