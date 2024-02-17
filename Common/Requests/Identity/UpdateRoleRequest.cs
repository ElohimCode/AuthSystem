namespace Common.Requests.Identity
{
    public class UpdateRoleRequest
    {
        public string RoleId { get; set; } = string.Empty; 
        public string RoleName { get; set; } = string.Empty;
        public string RoleDescription { get; set; } = string.Empty;
    }
}
