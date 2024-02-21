using Common.Responses.Models;

namespace Common.Requests.Identity.Users
{
    public class UpdateUserRolesRequest
    {
        public string UserId { get; set; } = null!;
        public List<UserRoleViewModel> Roles { get; set; } = [];
    }
}
