using Application.Contracts.Services.Identity;
using Application.Features.Identity.Roles.Commands;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Roles.Handlers
{
    public class DeleteRoleHandler(IRoleService roleService) : IRequestHandler<DeleteRoleCommand, IResponseWrapper>
    {
        public Task<IResponseWrapper> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            return roleService.DeleteAsync(request.RoleId);
        }
    }
}
