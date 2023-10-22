using Pattern.Application.Services.Authentication.Dtos;
using Pattern.Core.Responses;

namespace Pattern.Application.Services.Authentication
{
    public interface IAuthenticationService
    {

        Task<ResponseDto<AccessTokenDto>> CreateTokenAsync(LoginDto loginDto);

        Task<ResponseDto<AccessTokenDto>> CreateTokenByRefreshTokenAsync(RefreshTokenDto refreshToken);

        Task<ResponseDto<NoContentDto>> RevokeRefreshTokenAsync(RefreshTokenDto refreshToken);
    }
}
