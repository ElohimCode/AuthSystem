using Microsoft.AspNetCore.Identity;

namespace Persistence.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; } = string.Empty;
    }
}

