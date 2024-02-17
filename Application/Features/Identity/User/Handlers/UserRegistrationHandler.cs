using Application.Features.Identity.User.Commands;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.User.Handlers
{
    internal class UserRegistrationHandler : IRequestHandler<UserRegistrationCommand, IResponseWrapper>
    {
        public Task<IResponseWrapper> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
