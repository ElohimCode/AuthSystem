namespace Common.Requests.Identity.Roles
{
    public class CreateRoleRequest
    {
        public string RoleName { get; set; } = string.Empty;
        public string RoleDescription { get; set; } = string.Empty;
    }
}
