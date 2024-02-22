using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.User.Queries
{
    public class GetUserQuery : IRequest<IResponseWrapper>
    {
        public string Id { get; set; } = null!;
    }
}
