using Application.Contracts.Services.Identity;
using Application.Features.Identity.User.Commands;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.User.Handlers
{
    public class DeleteUserHandler(IUserService userService) : IRequestHandler<DeleteUserCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            return await userService.DeleteUserAsync(request.Id);
        }
    }
}
