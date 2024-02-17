using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Roles.Queries
{
    public class GetRoleByIdQuery : IRequest<IResponseWrapper>
    {
        public string roleId { get; set; } = string.Empty;
    }
}
