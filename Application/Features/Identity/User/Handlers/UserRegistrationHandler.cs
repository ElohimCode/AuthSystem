using Application.Contracts.Services.Identity;
using Application.Features.Identity.User.Commands;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.User.Handlers
{
    public class UserRegistrationHandler(IUserService userService) : IRequestHandler<UserRegistrationCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            return await userService.RegisterUserAsync(request);
        }
    }
}
