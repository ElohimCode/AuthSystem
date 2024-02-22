using Common.Requests.Identity.Users;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.User.Commands
{
    public class UpdateUserCommand : UpdateUserRequest, IRequest<IResponseWrapper>
    {
    }
}
