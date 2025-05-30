using Microsoft.AspNetCore.Mvc;
using Pattern.Application.Services.Authentication;
using Pattern.Application.Services.Authentication.Dtos;

namespace Pattern.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthenticationService authenticationService) : BaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> CreateToken(LoginDto loginDto)
    {
        var result = await authenticationService.CreateTokenAsync(loginDto);
        return Success(result);
    }


    [HttpDelete("revoke")]
    public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
    {
        await authenticationService.RevokeRefreshTokenAsync(refreshTokenDto);
        return Success();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var result = await authenticationService.CreateTokenByRefreshTokenAsync(refreshTokenDto);
        return Success(result);
    }
}