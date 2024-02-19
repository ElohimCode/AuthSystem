using Application.Contracts.Services.Identity;
using AutoMapper;
using Common.Authorization;
using Common.Requests.Identity;
using Common.Responses.Identity;
using Common.Responses.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Infrastructure.Services.Identity
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> CreateAsync(CreateRoleRequest request)
        {
            var roleExist = await _roleManager.FindByNameAsync(request.RoleName);
            if(roleExist is not null)
            {
                return await ResponseWrapper<string>.FailAsync($"Role with name: {request.RoleName.ToLower()} already exists.");
            }

            var roleEntity = new ApplicationRole
            {
                Name = request.RoleName,
                Description = request.RoleDescription
            };
            var result = await _roleManager.CreateAsync(roleEntity);
            if (result.Succeeded)
            {
                return await ResponseWrapper<string>
                    .SuccessAsync($"Role with name {request.RoleName.ToLower()} created successfully.");
            }
            return await ResponseWrapper<string>.FailAsync(GetIdentityResultErrorDescriptions(result));
        }

        public Task<IResponseWrapper> DeleteAsync(string roleId)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponseWrapper> GetAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            if (roles.Count > 0)
            {
                var mappedRoles = _mapper.Map<List<RoleResponse>>(roles);
                return await ResponseWrapper<List<RoleResponse>>.SuccessAsync(mappedRoles);
            }
            return await ResponseWrapper<string>.FailAsync("No roles were found.");
        }

        public async Task<IResponseWrapper> GetByIdAsync(string roleId)
        {
           var roleExist = await _roleManager.FindByIdAsync(roleId);
            if (roleExist is not null)
            {
                var mappedRole = _mapper.Map<RoleResponse>(roleExist);
                return await ResponseWrapper<RoleResponse> .SuccessAsync(mappedRole);
            }
            return await ResponseWrapper.FailAsync($"Role does not exist.");
        }

        public Task<IResponseWrapper> GetPermissionsAsync(string roleId)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponseWrapper> UpdateAsync(UpdateRoleRequest request)
        {
            var roleEntity = await _roleManager.FindByIdAsync(request.RoleId);

            if (roleEntity is null || roleEntity.Name is null)
                return await ResponseWrapper.FailAsync("Role does not exist.");

            if (roleEntity.Name.Equals(AppRoles.Admin))
                return await ResponseWrapper.FailAsync("Not authorized to update Admin role.");

            roleEntity.Name = request.RoleName;
            roleEntity.Description = request.RoleDescription;

            var identityResult = await _roleManager.UpdateAsync(roleEntity);
            if (identityResult.Succeeded)
                return await ResponseWrapper<string>.SuccessAsync($"Role is updated successfully.");
            return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescriptions(identityResult));
        }

        public Task<IResponseWrapper> UpdateRolePermissionsAsync(UpdateRolePermissionsRequest request)
        {
            throw new NotImplementedException();
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
