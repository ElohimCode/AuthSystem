using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.User.Commands
{
    public class DeleteUserCommand : IRequest<IResponseWrapper>
    {
        public string Id { get; set; } = null!;
    }
}
