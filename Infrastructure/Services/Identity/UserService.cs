using Application.Contracts.Services.Identity;
using AutoMapper;
using Common.Authorization;
using Common.Requests.Identity.Users;
using Common.Responses.Identity;
using Common.Responses.Models;
using Common.Responses.Wrappers;
using Microsoft.AspNetCore.Identity;
using Persistence.DataAccess.Identiy.Contracts;
using Persistence.Models;

namespace Infrastructure.Services.Identity
{
    public class UserService(IAuthenticationManager authenticationManager, IMapper mapper, ICurrentUserService currentUserService) : IUserService
    {
        public async Task<IResponseWrapper> DeleteUserAsync(string id)
        {
            // Check if user exist
            var userEntity = await authenticationManager.GetUserByIdAsync(id);

            if (userEntity is null)
            {
                return await ResponseWrapper.FailAsync("User does not exist.");
            }

            // Check if user is not an admin
            if (await authenticationManager.UserIsInRoleAsync(userEntity, AppRoles.Admin))
            {
                return await ResponseWrapper.FailAsync("Not authorized to perform operation.");
            }

            // Delete user
            var identityResult = await authenticationManager.DeleteUserAsync(userEntity);
            if (identityResult.Succeeded)
            {
                return await ResponseWrapper.SuccessAsync("User is deleted successfully.");
            }
            return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescription(identityResult));
        }

        public async Task<IResponseWrapper> GetRolesAsync(string id)
        {
            var userRoles = new List<UserRoleViewModel>();
            var userEntity = await authenticationManager.GetUserByIdAsync(id);

            if (userEntity is null)
            {
                return await ResponseWrapper.FailAsync("User does not exist");
            }

            var allRoles = await authenticationManager.GetAllRolesAsync();

            var userRolesVMs = new List<UserRoleViewModel>();
            foreach (var role in allRoles)
            {
                var isAssigned = await authenticationManager.UserIsInRoleAsync(userEntity, role.Name);
                userRolesVMs.Add(new UserRoleViewModel
                {
                    RoleName = role.Name!,
                    RoleDescription = role.Description,
                    IsAssignedToUser = isAssigned
                });
            }
            return await ResponseWrapper<List<UserRoleViewModel>>.SuccessAsync(userRolesVMs);
        }

        public Task<IResponseWrapper> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponseWrapper> GetUserByIdAsync(string id)
        {
            var userEntity = await authenticationManager.GetUserByIdAsync(id);
            if (userEntity is null)
            {
                return await ResponseWrapper.FailAsync("User does not exist.");
            }
            var mappedUser = mapper.Map<UserResponse>(userEntity);
            return await ResponseWrapper<UserResponse>.SuccessAsync(mappedUser);
        }

        public async Task<IResponseWrapper> GetUsersAsync()
        {
            var userEntity = await authenticationManager.GetUsersAsync();
            if (userEntity.Count > 0)
            {
                var mappedUsers = mapper.Map<List<UserResponse>>(userEntity);
                return await ResponseWrapper<List<UserResponse>>.SuccessAsync(mappedUsers);
            }
            return await ResponseWrapper.FailAsync("No users were found.");
        }

        public async Task<IResponseWrapper> RegisterUserAsync(UserRegistrationRequest request)
        {
            var userEmailExist = await authenticationManager.GetUserByEmailAsync(request.Email);

            if (userEmailExist is not null)
            {
                return await ResponseWrapper.FailAsync("Email already taken.");
            }

            var userNameExist = await authenticationManager.GetUserByUserNameAsync(request.UserName);

            if (userNameExist is not null)
            {
                return await ResponseWrapper.FailAsync("Username already taken.");
            }

            var userEntity = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                IsActive = request.ActivateUser,
                EmailConfirmed = request.AutoComfirmEmail
            };

            var password = new PasswordHasher<ApplicationUser>();
            userEntity.PasswordHash = password.HashPassword(userEntity, request.Password);

            var identityResult = await authenticationManager.CreateUserAsync(userEntity);
            if (identityResult.Succeeded)
            {
                await authenticationManager.AddToRoleAsync(userEntity, AppRoles.Basic);
                return await ResponseWrapper.SuccessAsync("User registered successfully.");
            }
            return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescription(identityResult));
        }

        public async Task<IResponseWrapper> UpdatePasswordAsync(UpdatePasswordRequest request)
        {
            var userEntity = await authenticationManager.GetUserByEmailAsync(request.Email);
            if (userEntity is null)
            {
                return await ResponseWrapper.FailAsync("User does not exist");
            }

            var identityResult = await authenticationManager
                .ChangePasswordAsync(userEntity, request.CurrentPassword, request.NewPassword);
            if (identityResult.Succeeded)
            {
                return await ResponseWrapper<string>.SuccessAsync("User password updated successfully.");
            }
            return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescription(identityResult));
        }

        public async Task<IResponseWrapper> UpdateRolesAsync(UpdateUserRolesRequest request)
        {
            var userEntity = await authenticationManager.GetUserByIdAsync(request.UserId);
            if (userEntity is null)
            {
                return await ResponseWrapper.FailAsync("User does not exist.");
            }

            if (userEntity.Email == AppCredentials.Email)
            {
                return await ResponseWrapper.FailAsync("Not authorized to perform this operation.");
            }
            var currentAssignedRoles = await authenticationManager.GetUserRolesAsync(userEntity);
            var rolesToBeAssigned = request.Roles
                .Where(role => role.IsAssignedToUser == true)
                .ToList();
            var currentLoggedInUser = await authenticationManager
                .GetUserByIdAsync(currentUserService.UserId);

            if (currentUserService is null)
            {
                return await ResponseWrapper.FailAsync("User does not exist.");
            }

            if (await authenticationManager.UserIsInRoleAsync(currentLoggedInUser!, AppRoles.Admin))
            {
                var identityResult1 = await authenticationManager.RemoveUserFromRolesAsync(userEntity, currentAssignedRoles);
                if (identityResult1.Succeeded)
                {
                    var identityResult = await authenticationManager
                        .AddUserToRolesAsync(userEntity, rolesToBeAssigned.Select(role => role.RoleName));
                    if(identityResult.Succeeded)
                    {
                        return await ResponseWrapper<string>.SuccessAsync("User roles updated successfully.");
                    }
                    return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescription(identityResult));
                }
                return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescription(identityResult1));

            }
            return await ResponseWrapper.FailAsync("Not authorized to perform the operation.");


        }

        public async Task<IResponseWrapper> UpdateUserAsync(UpdateUserRequest request)
        {
            var userEntity = await authenticationManager.GetUserByIdAsync(request.UserId);
            if (userEntity is null)
            {
                return await ResponseWrapper.FailAsync("User does not exist.");
            }
            userEntity.FirstName = request.FirstName;
            userEntity.LastName = request.LastName;
            userEntity.PhoneNumber = request.PhoneNumber;

            var identityResult = await authenticationManager.UpdateUserAsync(userEntity);
            if (identityResult.Succeeded)
            {
                return await ResponseWrapper.SuccessAsync("User details updated successfully.");
            }
            return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescription(identityResult));
        }

        public Task<IResponseWrapper> UpdateUserStatusAsync(UpdateUserStatusRequest request)
        {
            throw new NotImplementedException();
        }

        private List<string> GetIdentityResultErrorDescription(IdentityResult identityResult)
        {
            var errorDescription = new List<string>();
            foreach (var error in identityResult.Errors)
            {
                errorDescription.Add(error.Description);
            }
            return errorDescription;
        }
    }
}
