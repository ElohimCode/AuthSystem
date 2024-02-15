using Application.Contracts.Services.Identity;
using Application.Features.Identity.User.Queries;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.User.Handlers
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenQuery, IResponseWrapper>
    {
        private readonly ITokenService _tokenService;

        public RefreshTokenHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<IResponseWrapper> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            return await _tokenService.GetRefreshTokenAsync(request);
        }
    }
}
