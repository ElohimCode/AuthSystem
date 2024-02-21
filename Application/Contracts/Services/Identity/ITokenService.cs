using Common.Requests.Identity.Tokens;
using Common.Responses.Identity;
using Common.Responses.Wrappers;

namespace Application.Contracts.Services.Identity
{
    public interface ITokenService
    {
        Task<ResponseWrapper<TokenResponse>> GetTokenAsync(TokenRequest request);
        Task<ResponseWrapper<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest request);
    }
}
