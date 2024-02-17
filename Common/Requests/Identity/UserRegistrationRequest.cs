namespace Common.Requests.Identity
{
    public class UserRegistrationRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ComfirmPassword { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool ActivateUser { get; set; }
        public bool AutoComfirmEmail { get; set; }
    }
}
