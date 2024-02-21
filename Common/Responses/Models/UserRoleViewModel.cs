namespace Common.Responses.Models
{
    public class UserRoleViewModel
    {
        public string RoleName { get; set; } = null!;
        public string RoleDescription { get; set; } = null!;
        public bool IsAssignedToUser { get; set; }
    }
}
