using Application.Contracts.Services.Identity;
using Application.Features.Identity.User.Commands;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.User.Handlers
{
    public class UpdateUserRolesHandler(IUserService userService) : IRequestHandler<UpdateUserRolesCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
        {
            return await userService.UpdateRolesAsync(request);
        }
    }
}
