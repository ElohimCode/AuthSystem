using Common.Requests.Identity;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.User.Commands
{
    public class UserRegistrationCommand : UserRegistrationRequest, IRequest<IResponseWrapper>
    {
    }
}
