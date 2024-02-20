using Application.Contracts.Services.Identity;
using Application.Features.Identity.Roles.Commands;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Roles.Handlers
{
    public class UpdateRolePermissionsHandler(IRoleService roleService) : IRequestHandler<UpdateRolePermissionsCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            return await roleService.UpdateRolePermissionsAsync(request);
        }
    }
}
