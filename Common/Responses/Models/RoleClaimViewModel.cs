namespace Common.Responses.Models
{
    public class RoleClaimViewModel
    {
        public string RoleId { get; set; } = string.Empty;
        public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public bool IsAssignedToRole { get; set; }
    }
}
