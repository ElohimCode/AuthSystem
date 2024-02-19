using Application.Contracts.Services.Identity;
using Application.Features.Identity.Roles.Commands;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Roles.Handlers
{
    public class CreateRoleHandler(IRoleService roleService) : IRequestHandler<CreateRoleCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            return await roleService.CreateAsync(request);
        }
    }
}
