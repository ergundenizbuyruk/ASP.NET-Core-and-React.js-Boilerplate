using Pattern.Application.Services.Authentication.Dtos;
using Pattern.Application.Services.Base;
using Pattern.Core.Responses;

namespace Pattern.Application.Services.Authentication
{
    public interface IAuthenticationService : IApplicationService
    {
        Task<AccessTokenDto> CreateTokenAsync(LoginDto loginDto);

        Task<AccessTokenDto> CreateTokenByRefreshTokenAsync(RefreshTokenDto refreshToken);

        Task RevokeRefreshTokenAsync(RefreshTokenDto refreshToken);
    }
}