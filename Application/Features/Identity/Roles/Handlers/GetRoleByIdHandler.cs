using Application.Contracts.Services.Identity;
using Application.Features.Identity.Roles.Queries;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Roles.Handlers
{
    public class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, IResponseWrapper>
    {
        private readonly IRoleService _roleService;

        public GetRoleByIdHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IResponseWrapper> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            return await _roleService.GetByIdAsync(request.roleId);
        }
    }
}
