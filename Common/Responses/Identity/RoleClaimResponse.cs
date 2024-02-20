using Common.Responses.Models;

namespace Common.Responses.Identity
{
    public class RoleClaimResponse
    {
        public RoleResponse Role { get; set; } = null!;
        public List<RoleClaimViewModel> RoleClaims { get; set; } = null!;
    }
}
