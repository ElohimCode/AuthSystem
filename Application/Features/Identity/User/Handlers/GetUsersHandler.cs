using Application.Contracts.Services.Identity;
using Application.Features.Identity.User.Queries;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.User.Handlers
{
    public class GetUsersHandler(IUserService userService) : IRequestHandler<GetUsersQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await userService.GetUsersAsync();
        }
    }
}
