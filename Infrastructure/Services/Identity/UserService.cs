using Application.Contracts.Services.Identity;
using AutoMapper;
using Common.Authorization;
using Common.Requests.Identity.Users;
using Common.Responses.Identity;
using Common.Responses.Wrappers;
using Microsoft.AspNetCore.Identity;
using Persistence.DataAccess.Identiy.Contracts;
using Persistence.Models;

namespace Infrastructure.Services.Identity
{
    public class UserService(IAuthenticationManager authenticationManager, IMapper mapper) : IUserService
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

        public Task<IResponseWrapper> GetRolesAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IResponseWrapper> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IResponseWrapper> GetUserByIdAsync(string id)
        {
            throw new NotImplementedException();
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

        public Task<IResponseWrapper> UpdatePasswordAsync(UpdatePasswordRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IResponseWrapper> UpdateRolesAsync(UpdateUserRolesRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IResponseWrapper> UpdateUserAsync(UpdateUserRequest request)
        {
            throw new NotImplementedException();
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
