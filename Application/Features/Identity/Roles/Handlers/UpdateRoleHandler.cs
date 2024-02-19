using Application.Contracts.Services.Identity;
using Application.Features.Identity.Roles.Commands;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Roles.Handlers
{
    public class UpdateRoleHandler(IRoleService roleService) : IRequestHandler<UpdateRoleCommand, IResponseWrapper>
    {
        public Task<IResponseWrapper> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            return roleService.UpdateAsync(request);
        }
    }
}
