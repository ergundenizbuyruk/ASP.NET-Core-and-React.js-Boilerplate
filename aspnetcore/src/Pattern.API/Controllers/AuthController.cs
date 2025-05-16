using Microsoft.AspNetCore.Mvc;
using Pattern.Application.Services.Authentication;
using Pattern.Application.Services.Authentication.Dtos;

namespace Pattern.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> CreateToken(LoginDto loginDto)
        {
            var result = await _authenticationService.CreateTokenAsync(loginDto);
            return ActionResultInstance(result);
        }


        [HttpDelete("revoke")]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authenticationService.RevokeRefreshTokenAsync(refreshTokenDto);
            return ActionResultInstance(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)

        {
            var result = await _authenticationService.CreateTokenByRefreshTokenAsync(refreshTokenDto);
            return ActionResultInstance(result);
        }
    }
}
