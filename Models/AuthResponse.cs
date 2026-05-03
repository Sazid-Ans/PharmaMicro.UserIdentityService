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

        public AuthResponse()
        {
            
        }
        public AuthResponse(bool isSuccess, string message, string token, string email)
        {
            IsSuccess = isSuccess;
            Message = message;
            Token = token;
            Email = email;
        }
    }
}
