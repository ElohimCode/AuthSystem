using Microsoft.AspNetCore.Identity;
using Persistence.Models;
using System.Security.Claims;

namespace Persistence.DataAccess.Identiy.Contracts
{
    public interface IAuthenticationManager
    {
        Task<ApplicationRole?> GetRoleByNameAsync(string roleName);
        Task<ApplicationRole?> GetRoleByIdAsync(string roleId);
        Task<List<ApplicationRole>> GetAllRolesAsync();
        Task<IdentityResult> CreateRoleAsync(ApplicationRole role);
        Task<IdentityResult> UpdateRoleAsync(ApplicationRole role);
        Task<IdentityResult> DeleteRoleAsync(ApplicationRole role);
        Task<IList<Claim>>  GetAllClaimsAsync(ApplicationRole role);
        Task<IdentityResult> RemoveClaimAsync(ApplicationRole role, Claim claim);
        Task<List<ApplicationUser>> GetUsersAsync();
        Task<bool> UserIsInRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user);
        Task<ApplicationUser?> GetUserByIdAsync(string Id);
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
        Task<ApplicationUser?> GetUserByUserNameAsync(string username);
        Task<IdentityResult> DeleteUserAsync(ApplicationUser user);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<IList<Claim>> GetUserClaimsAsync(ApplicationUser user);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
    }
}
