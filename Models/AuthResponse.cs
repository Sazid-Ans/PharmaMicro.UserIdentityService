namespace PharmaMicro.UserIdentityService.Models
{
    /// <summary>
    /// Response class send back to the react page.
    /// </summary>
    public class AuthResponse
    {
        public bool IsSuccess {  get; set; }
        public string Message { get; set; }
        public string Token { get; set; } = string.Empty;

        public AuthResponse()
        {
            
        }
        public AuthResponse(bool isSuccess, string message, string token)
        {
            IsSuccess = isSuccess;
            Message = message;
            Token = token;
        }
    }
}
