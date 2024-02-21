using Common.Responses.Models;

namespace Common.Requests.Identity.Roles
{
    public class UpdateRolePermissionsRequest
    {
        public string RoleId { get; set; } = string.Empty;
        public List<RoleClaimViewModel> RoleClaims { get; set; } = [];
    }
}
