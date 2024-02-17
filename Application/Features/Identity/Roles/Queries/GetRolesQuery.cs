using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Roles.Queries
{
    public class GetRolesQuery : IRequest<IResponseWrapper>
    {
    }
}
