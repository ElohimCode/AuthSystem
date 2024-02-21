using Application.Contracts.Services.Identity;
using Common.Authorization;
using Common.Requests.Identity.Users;
using Common.Responses.Wrappers;
using Microsoft.AspNetCore.Identity;
using Persistence.DataAccess.Identiy.Contracts;
using Persistence.Models;

namespace Infrastructure.Services.Identity
{
    public class UserService(IAuthenticationManager authenticationManager) : IUserService
    {
        public Task<IResponseWrapper> DeleteUserAsync(string id)
        {
            throw new NotImplementedException();
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

        public Task<IResponseWrapper> GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IResponseWrapper> RegisterUserAsync(UserRegistrationRequest request)
        {
            var userEmailExist = await authenticationManager.GetUserByEmailAsync(request.Email);
            
            if (userEmailExist is not  null)
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
