using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pattern.Application.Services.Authentication.Dtos;
using Pattern.Application.Services.Base;
using Pattern.Core.Entites.Authentication;
using Pattern.Core.Exceptions;
using Pattern.Persistence.Repositories;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Authentication;

public class AuthenticationService(
    IUnitOfWork unitOfWork,
    IMapper objectMapper,
    ITokenService tokenService,
    UserManager<User> userManager,
    IRepository<UserRefreshToken, int> userRefreshTokenRepository,
    SignInManager<User> signInManager)
    : ApplicationService(unitOfWork, objectMapper), IAuthenticationService
{
    public async Task<AccessTokenDto> CreateTokenAsync(LoginDto loginDto)
    {
        var user = await userManager.Users
            .Where(p => p.UserName == loginDto.Email || p.Email == loginDto.Email)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            throw new BadRequestException("E-posta veya Parola hatalı");
        }

        if (!user.IsActive)
        {
            throw new UserIsNotActiveException("Kullanıcı pasif durumundadır.");
        }

        var result = await signInManager.PasswordSignInAsync(user, loginDto.Password, false, true);

        if (result.IsLockedOut)
        {
            throw new UserIsLockedOutException(
                "Hesabınız kilitli durumdadır. Daha sonra giriş yapmayı deneyiniz.");
        }

        if (result.IsNotAllowed)
        {
            throw new UserNotVerifiedException("Lütfen önce e-posta adresinizi onaylayınız.");
        }

        if (!result.Succeeded)
        {
            throw new BadRequestException("E-posta veya Parola hatalı");
        }

        var token = await tokenService.CreateTokenAsync(user);
        var userRefreshToken = await userRefreshTokenRepository.GetAll().Where(x => x.UserId == user.Id)
            .SingleOrDefaultAsync();

        if (userRefreshToken == null)
        {
            await userRefreshTokenRepository.CreateAsync(new UserRefreshToken
            {
                UserId = user.Id,
                Code = token.RefreshToken,
                Expiration =
                    token.RefreshTokenExpiration
            });
        }
        else
        {
            userRefreshToken.Code = token.RefreshToken;
            userRefreshToken.Expiration = token.RefreshTokenExpiration;
            userRefreshTokenRepository.Update(userRefreshToken);
        }

        await SaveChangesAsync();
        return token;
    }

    public async Task<AccessTokenDto> CreateTokenByRefreshTokenAsync(RefreshTokenDto refreshToken)
    {
        var existRefreshToken = await userRefreshTokenRepository.GetAll()
            .Where(x => x.Code == refreshToken.Token).SingleOrDefaultAsync();

        if (existRefreshToken == null)
        {
            throw new NotFoundException("Refresh Token Hatalı");
        }

        var user = await userManager.FindByIdAsync(existRefreshToken.UserId.ToString());

        if (user is null)
        {
            throw new NotFoundException("Kullanıcı Bulunamadı");
        }

        if (existRefreshToken.Expiration < DateTimeOffset.UtcNow)
        {
            throw new NotFoundException("Lütfen Tekrar Giriş Yapınız.");
        }

        var tokenDto = await tokenService.CreateTokenAsync(user);
        existRefreshToken.Code = tokenDto.RefreshToken;
        existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

        userRefreshTokenRepository.Update(existRefreshToken);
        await SaveChangesAsync();
        return tokenDto;
    }

    public async Task RevokeRefreshTokenAsync(RefreshTokenDto refreshToken)
    {
        var existRefreshToken = await userRefreshTokenRepository.GetAll()
            .Where(x => x.Code == refreshToken.Token)
            .SingleOrDefaultAsync();

        if (existRefreshToken is null)
        {
            throw new NotFoundException("Refresh Token Bulunamadı.");
        }

        userRefreshTokenRepository.Delete(existRefreshToken);
        await SaveChangesAsync();
    }
}