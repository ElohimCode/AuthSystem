using Common.Requests.Identity;
using Common.Responses.Wrappers;

namespace Application.Contracts.Services.Identity
{
    public interface IRoleService
    {
        Task<IResponseWrapper> CreateAsync(CreateRoleRequest request);
        Task<IResponseWrapper> UpdateAsync(UpdateRoleRequest request);
        Task<IResponseWrapper> DeleteAsync(string roleId);
        Task<IResponseWrapper> GetAsync();
        Task<IResponseWrapper> GetByIdAsync(string roleId);
        Task<IResponseWrapper> GetPermissionsAsync(string roleId);
        Task<IResponseWrapper> UpdateRolePermissionsAsync(UpdateRolePermissionsRequest request);
    }
}
