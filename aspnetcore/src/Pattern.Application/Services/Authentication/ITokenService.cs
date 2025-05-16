using Pattern.Application.Services.Authentication.Dtos;
using Pattern.Application.Services.Base;
using Pattern.Core.Entites.Authentication;

namespace Pattern.Application.Services.Authentication
{
    public interface ITokenService : IApplicationService
    {
        Task<AccessTokenDto> CreateTokenAsync(User userApp);
    }
}