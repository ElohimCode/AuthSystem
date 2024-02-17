using Application.Contracts.Services.Identity;
using Application.Features.Identity.Roles.Commands;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Roles.Handlers
{
    public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, IResponseWrapper>
    {
        private readonly IRoleService _roleService;

        public CreateRoleHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IResponseWrapper> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            return await _roleService.CreateAsync(request);
        }
    }
}
