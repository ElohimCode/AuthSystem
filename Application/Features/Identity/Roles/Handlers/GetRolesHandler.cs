using Application.Contracts.Services.Identity;
using Application.Features.Identity.Roles.Queries;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Roles.Handlers
{
    public class GetRolesHandler(IRoleService roleService) : IRequestHandler<GetRolesQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            return await roleService.GetAsync();
        }
    }
}
