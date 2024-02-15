using Application.Contracts.Services.Identity;
using Application.Features.Identity.User.Queries;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Identity.User.Handlers
{
    public class LoginHandler : IRequestHandler<LoginQuery, IResponseWrapper>
    {
        private readonly ITokenService _tokenService;
        public LoginHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public async Task<IResponseWrapper> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            return await _tokenService.GetTokenAsync(request);
        }
    }
}
