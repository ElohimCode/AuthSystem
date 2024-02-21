using Common.Requests.Identity.Tokens;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.User.Queries
{
    public class LoginQuery : TokenRequest, IRequest<IResponseWrapper>
    { }
}
