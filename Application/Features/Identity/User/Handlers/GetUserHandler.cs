using Application.Contracts.Services.Identity;
using Application.Features.Identity.User.Queries;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.User.Handlers
{
    public class GetUserHandler(IUserService userService) : IRequestHandler<GetUserQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await userService.GetUserByIdAsync(request.Id);
        }
    }
}
