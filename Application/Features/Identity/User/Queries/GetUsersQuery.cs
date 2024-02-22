using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.User.Queries
{
    public class GetUsersQuery : IRequest<IResponseWrapper>
    {
    }
}
