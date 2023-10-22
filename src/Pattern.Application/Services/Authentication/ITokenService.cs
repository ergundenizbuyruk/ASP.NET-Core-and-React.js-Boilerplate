using Pattern.Application.Services.Authentication.Dtos;
using Pattern.Core.Entites.Authentication;

namespace Pattern.Application.Services.Authentication
{
    public interface ITokenService
    {
        Task<AccessTokenDto> CreateTokenAsync(User userApp);
    }
}
