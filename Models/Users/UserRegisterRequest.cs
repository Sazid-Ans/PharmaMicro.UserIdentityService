namespace PharmaMicro.UserIdentityService.Models.Users
{
    /// <summary>
    /// Request class for registration input.
    /// </summary>
    public class UserRegisterRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty ;
        public string Password { get; set; } = string.Empty;
    }
}
