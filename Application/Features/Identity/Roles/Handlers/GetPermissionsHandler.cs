using Application.Contracts.Services.Identity;
using Application.Features.Identity.Roles.Queries;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Roles.Handlers
{
    public class GetPermissionsHandler(IRoleService roleService) : IRequestHandler<GetPermissionsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            return await roleService.GetPermissionsAsync(request.RoleId);
        }
    }
}
