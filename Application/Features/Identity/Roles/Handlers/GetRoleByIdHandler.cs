using Application.Contracts.Services.Identity;
using Application.Features.Identity.Roles.Queries;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Roles.Handlers
{
    public class GetRoleByIdHandler(IRoleService roleService) : IRequestHandler<GetRoleByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            return await roleService.GetByIdAsync(request.RoleId);
        }
    }
}
