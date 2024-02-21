using Application.Contracts.Services.Identity;
using AutoMapper;
using Common.Authorization;
using Common.Requests.Identity.Roles;
using Common.Responses.Identity;
using Common.Responses.Models;
using Common.Responses.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.DataAccess.Identiy.Contracts;
using Persistence.Models;

namespace Infrastructure.Services.Identity
{
    public class RoleService(IAuthenticationManager authenticationManager, IMapper mapper, ApplicationDbContext context) : IRoleService
    {
        public async Task<IResponseWrapper> CreateAsync(CreateRoleRequest request)
        {
            var roleExist = await authenticationManager.GetRoleByNameAsync(request.RoleName);
            if (roleExist is not null)
            {
                return await ResponseWrapper<string>.FailAsync($"Role with name: {request.RoleName.ToLower()} already exists.");
            }

            var roleEntity = new ApplicationRole
            {
                Name = request.RoleName,
                Description = request.RoleDescription
            };
            var result = await authenticationManager.CreateRoleAsync(roleEntity);
            if (result.Succeeded)
            {
                return await ResponseWrapper<string>
                    .SuccessAsync($"Role with name {request.RoleName.ToLower()} created successfully.");
            }
            return await ResponseWrapper<string>.FailAsync(GetIdentityResultErrorDescriptions(result));
        }

        public async Task<IResponseWrapper> DeleteAsync(string roleId)
        {
            var roleEntity = await authenticationManager.GetRoleByIdAsync(roleId);
            if (roleEntity is null)
                return await ResponseWrapper.FailAsync("Role does not exist");

            if (roleEntity.Name is not null && roleEntity.Name.Equals(AppRoles.Admin))
                return await ResponseWrapper<string>.FailAsync("Not authorized to perform this operation.");

            var allUsers = await authenticationManager.GetUsersAsync();

            if (allUsers.Exists(user => authenticationManager.UserIsInRoleAsync(user, roleEntity.Name!).Result))
                return await ResponseWrapper.FailAsync($"Role: {roleEntity.Name} is currently assigned to a user.");


            var identityResult = await authenticationManager.DeleteRoleAsync(roleEntity);
            if (identityResult.Succeeded)
            {
                return await ResponseWrapper
                    .SuccessAsync($"Role with name: {roleEntity.Name} is deleted successfully.");
            }
            return await ResponseWrapper
                .FailAsync(GetIdentityResultErrorDescriptions(identityResult));
        }

        public async Task<IResponseWrapper> GetAsync()
        {
            var roles = await authenticationManager.GetAllRolesAsync();
            if (roles.Count > 0)
            {
                var mappedRoles = mapper.Map<List<RoleResponse>>(roles);
                return await ResponseWrapper<List<RoleResponse>>.SuccessAsync(mappedRoles);
            }
            return await ResponseWrapper<string>.FailAsync("No roles were found.");
        }

        public async Task<IResponseWrapper> GetByIdAsync(string roleId)
        {
            var roleExist = await authenticationManager.GetRoleByIdAsync(roleId);
            if (roleExist is not null)
            {
                var mappedRole = mapper.Map<RoleResponse>(roleExist);
                return await ResponseWrapper<RoleResponse>.SuccessAsync(mappedRole);
            }
            return await ResponseWrapper.FailAsync($"Role does not exist.");
        }

        public async Task<IResponseWrapper> GetPermissionsAsync(string roleId)
        {
            var roleEntity = await authenticationManager.GetRoleByIdAsync(roleId);
            if (roleEntity is null)
                return await ResponseWrapper<RoleClaimResponse>.FailAsync("Role does not exist");

            var permissions = AppPermissions.AllPermissions;
            var currentRoleClaims = await GetAllClaimsForRoleAsync(roleId);

            var currentlyAssignedPermissions = permissions
                .Where(permission => currentRoleClaims.Any(crc => crc.ClaimValue == permission.Name))
                .Select(permission => new RoleClaimViewModel
                {
                    RoleId = roleId,
                    ClaimType = AppClaim.Permission,
                    ClaimValue = permission.Name,
                    Description = permission.Description,
                    Group = permission.Group,
                    IsAssignedToRole = true
                }).ToList();

            var unAssignedPermissions = permissions
                .Where(permission => !currentlyAssignedPermissions.Any(cap => cap.ClaimValue == permission.Name))
                .Select(permission => new RoleClaimViewModel
                {
                    RoleId = roleId,
                    ClaimType = AppClaim.Permission,
                    ClaimValue = permission.Name,
                    Description = permission.Description,
                    Group = permission.Group,
                    IsAssignedToRole = false
                }).ToList();
            var roleClaimResponse = new RoleClaimResponse
            {
                Role = new RoleResponse
                {
                    Id = roleId,
                    Name = roleEntity.Name!,
                    Description = roleEntity.Description
                },
                RoleClaims = [.. currentlyAssignedPermissions, .. unAssignedPermissions]
            };
            return await ResponseWrapper<RoleClaimResponse>
                .SuccessAsync(roleClaimResponse);
        }

        private async Task<List<RoleClaimViewModel>> GetAllClaimsForRoleAsync(string roleId)
        {
            var roleClaims = await context.RoleClaims
                .Where(rc => rc.RoleId.Equals(roleId))
                .ToListAsync();

            if (roleClaims.Any())
            {
                var mappedClaims = mapper.Map<List<RoleClaimViewModel>>(roleClaims);
                return mappedClaims;
            }
            return [];
        }

        public async Task<IResponseWrapper> UpdateAsync(UpdateRoleRequest request)
        {
            var roleEntity = await authenticationManager.GetRoleByIdAsync(request.RoleId);

            if (roleEntity is null)
                return await ResponseWrapper.FailAsync("Role does not exist.");

            if (roleEntity.Name is not null && roleEntity.Name.Equals(AppRoles.Admin))
                return await ResponseWrapper.FailAsync("Not authorized to update Admin role.");

            roleEntity.Name = request.RoleName;
            roleEntity.Description = request.RoleDescription;

            var identityResult = await authenticationManager.UpdateRoleAsync(roleEntity);
            if (identityResult.Succeeded)
                return await ResponseWrapper<string>.SuccessAsync($"Role is updated successfully.");
            return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescriptions(identityResult));
        }

        public async Task<IResponseWrapper> UpdateRolePermissionsAsync(UpdateRolePermissionsRequest request)
        {
            var roleEntity = await authenticationManager.GetRoleByIdAsync(request.RoleId);
            if (roleEntity is null)
                return await ResponseWrapper.FailAsync("Role does not exist.");
            if (roleEntity.Name!.Equals(AppRoles.Admin))
                return await ResponseWrapper<string>
                    .FailAsync("Not authorized to change permissions for Admin user");

            var permissionsToBeAssigned = request.RoleClaims
                .Where(rc => rc.IsAssignedToRole == true)
                .ToList();

            var currentlyAssignedClaims = await authenticationManager.GetAllClaimsAsync(roleEntity);

            foreach (var claim in currentlyAssignedClaims)
            {
                 await authenticationManager.RemoveClaimAsync(roleEntity, claim);
            }

            foreach (var claim in permissionsToBeAssigned)
            {
                var mappedRoleClaim = mapper.Map<ApplicationRoleClaim>(claim);
                await context.RoleClaims.AddAsync(mappedRoleClaim);
            }
            await context.SaveChangesAsync();
            return await ResponseWrapper<string>
                .SuccessAsync("Role permissions updated successfully");
        }

        private List<string> GetIdentityResultErrorDescriptions(IdentityResult result)
        {
            var errorDescriptions = new List<string>();
            foreach (var error in result.Errors)
            {
                errorDescriptions.Add(error.Description);
            }
            return errorDescriptions;
        }
    }
}
