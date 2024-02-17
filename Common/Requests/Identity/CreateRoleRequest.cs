namespace Common.Requests.Identity
{
    public class CreateRoleRequest
    {
        public string RoleName { get; set; } = string.Empty;
        public string RoleDescription { get; set; } = string.Empty;
    }
}
