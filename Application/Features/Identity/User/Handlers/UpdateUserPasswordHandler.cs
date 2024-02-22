using Application.Contracts.Services.Identity;
using Application.Features.Identity.User.Commands;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.User.Handlers
{
    public class UpdateUserPasswordHandler(IUserService userService) : IRequestHandler<UpdateUserPasswordCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            return await userService.UpdatePasswordAsync(request);
        }
    }
}
