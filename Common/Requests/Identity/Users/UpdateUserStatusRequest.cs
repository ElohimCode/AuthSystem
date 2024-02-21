namespace Common.Requests.Identity.Users
{
    public class UpdateUserStatusRequest
    {
        public string UserId { get; set; } = null!;
        public bool Activate { get; set; }
    }
}
