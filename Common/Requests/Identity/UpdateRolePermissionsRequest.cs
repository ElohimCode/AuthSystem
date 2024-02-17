using Common.Responses.Models;

namespace Common.Requests.Identity
{
    public class UpdateRolePermissionsRequest
    {
        public string RoleId { get; set; } = string.Empty;
        public List<RoleClaimViewModel> RoleClaims { get; set; } = [];
    }
}
