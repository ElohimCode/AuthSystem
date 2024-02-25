using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.User.Queries
{
    public class GetUserRolesQuery(string UserId) : IRequest<IResponseWrapper>
    {
        public string UserId { get; } = UserId;
    }
}
