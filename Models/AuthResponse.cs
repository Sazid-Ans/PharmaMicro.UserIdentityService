namespace PharmaMicro.UserIdentityService.Models
{
    /// <summary>
    /// Response class send back to the react page.
    /// </summary>
    public class AuthResponse
    {
        public bool IsSuccess {  get; set; }
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
