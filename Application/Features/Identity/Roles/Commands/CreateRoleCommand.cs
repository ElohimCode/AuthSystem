using Common.Requests.Identity;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.Roles.Commands
{
    public class CreateRoleCommand : CreateRoleRequest, IRequest<IResponseWrapper>
    {
    }
}
