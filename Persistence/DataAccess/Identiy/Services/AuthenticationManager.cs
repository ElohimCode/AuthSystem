using Application.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.DataAccess.Identiy.Contracts;
using Persistence.Models;
using System.Security.Claims;

namespace Persistence.DataAccess.Identiy.Services
{
    public class AuthenticationManager(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager) : IAuthenticationManager
    {
        public Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> CreateRoleAsync(ApplicationRole role)
        {
            return await roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(ApplicationRole role)
        {
            return await roleManager.DeleteAsync(role);
        }

        public async Task<IList<Claim>> GetAllClaimsAsync(ApplicationRole role)
        {
            return await roleManager.GetClaimsAsync(role);
        }

        public async Task<List<ApplicationRole>> GetAllRolesAsync()
        {
            return await roleManager.Roles.ToListAsync();
        }

        public async Task<ApplicationRole?> GetRoleByIdAsync(string roleId)
        {

            return await roleManager.FindByIdAsync(roleId);
        }

        public async Task<ApplicationRole?> GetRoleByNameAsync(string roleName)
        {
            return await roleManager.FindByNameAsync(roleName);
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<IList<Claim>> GetUserClaimsAsync(ApplicationUser user)
        {
            return await userManager.GetClaimsAsync(user);
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await userManager.GetRolesAsync(user);
        }

        public async Task<List<ApplicationUser>> GetUsersAsync()
        {
            return await userManager.Users.ToListAsync();
        }

        public async Task<IdentityResult> RemoveClaimAsync(ApplicationRole role, Claim claim)
        {
            return await roleManager.RemoveClaimAsync(role, claim);
        }

        public async Task<IdentityResult> UpdateRoleAsync(ApplicationRole role)
        {
            return await roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
        {
            return await userManager.UpdateAsync(user);
        }

        public async Task<bool> UserIsInRoleAsync(ApplicationUser user, string role)
        {
            return await userManager.IsInRoleAsync(user, role);
        }
    }
}
