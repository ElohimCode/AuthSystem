using Application.Contracts.Services.Identity;
using Application.Features.Identity.User.Queries;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.User.Handlers
{
    public class GetUserRolesHandler(IUserService userService) : IRequestHandler<GetUserRolesQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            return await userService.GetRolesAsync(request.UserId);
        }
    }
}
