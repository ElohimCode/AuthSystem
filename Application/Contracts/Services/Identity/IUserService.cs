using Common.Requests.Identity.Users;
using Common.Responses.Wrappers;

namespace Application.Contracts.Services.Identity
{
    public interface IUserService
    {
        Task<IResponseWrapper> RegisterUserAsync(UserRegistrationRequest request);
        Task<IResponseWrapper> GetUserByIdAsync(string id);
        Task<IResponseWrapper> GetUsersAsync();
        Task<IResponseWrapper> UpdateUserAsync(UpdateUserRequest request);
        Task<IResponseWrapper> UpdatePasswordAsync(UpdatePasswordRequest request);
        Task<IResponseWrapper> UpdateUserStatusAsync(UpdateUserStatusRequest request);
        Task<IResponseWrapper> GetRolesAsync(string id);
        Task<IResponseWrapper> UpdateRolesAsync(UpdateUserRolesRequest request);
        Task<IResponseWrapper> GetUserByEmailAsync(string email);
        Task<IResponseWrapper> DeleteUserAsync(string id);
    }
}
