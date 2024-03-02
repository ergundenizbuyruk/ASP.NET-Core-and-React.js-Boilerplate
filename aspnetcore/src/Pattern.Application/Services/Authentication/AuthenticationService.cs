using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pattern.Application.Services.Authentication.Dtos;
using Pattern.Application.Services.Base;
using Pattern.Core.Entites.Authentication;
using Pattern.Core.Responses;
using Pattern.Persistence.Repositories;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Authentication
{
	public class AuthenticationService : ApplicationService, IAuthenticationService
	{
		private readonly ITokenService tokenService;
		private readonly UserManager<User> userManager;
		private readonly SignInManager<User> signInManager;
		private readonly IRepository<UserRefreshToken, int> userRefreshTokenRepository;

		public AuthenticationService(IUnitOfWork unitOfWork, IMapper objectMapper, ITokenService tokenService, UserManager<User> userManager,
			IRepository<UserRefreshToken, int> userRefreshTokenRepository, SignInManager<User> signInManager) : base(unitOfWork, objectMapper)
		{
			this.tokenService = tokenService;
			this.userManager = userManager;
			this.userRefreshTokenRepository = userRefreshTokenRepository;
			this.signInManager = signInManager;
		}

		public async Task<ResponseDto<AccessTokenDto>> CreateTokenAsync(LoginDto loginDto)
		{
			var user = await userManager.FindByEmailAsync(loginDto.Email);

			if (user == null)
			{
				return ResponseDto<AccessTokenDto>.Fail("E-posta veya Parola hatalı", 400);
			}

			if (!user.IsActive)
			{
				return ResponseDto<AccessTokenDto>.Fail("Kullanıcı pasif durumundadır.", 400);
			}

			var result = await signInManager.PasswordSignInAsync(user, loginDto.Password, false, true);

			if (result.IsLockedOut)
			{
				return ResponseDto<AccessTokenDto>.Fail("Hesabınız kilitli durumdadır. Daha sonra giriş yapmayı deneyiniz.", 400);
			}

			if (result.IsNotAllowed)
			{
				return ResponseDto<AccessTokenDto>.Fail("Lütfen önce e-posta adresinizi onaylayınız.", 400);
			}

			if (!result.Succeeded)
			{
				return ResponseDto<AccessTokenDto>.Fail("E-posta veya Parola hatalı", 400);
			}

			var token = await tokenService.CreateTokenAsync(user);
			var userRefreshToken = await userRefreshTokenRepository.GetAll().Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

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
			return ResponseDto<AccessTokenDto>.Success(token, 200);
		}

		public async Task<ResponseDto<AccessTokenDto>> CreateTokenByRefreshTokenAsync(RefreshTokenDto refreshToken)
		{
			var existRefreshToken = await userRefreshTokenRepository.GetAll()
				.Where(x => x.Code == refreshToken.Token).SingleOrDefaultAsync();

			if (existRefreshToken == null)
			{
				return ResponseDto<AccessTokenDto>.Fail("Refresh Token Hatalı", 404, false);
			}

			var user = await userManager.FindByIdAsync(existRefreshToken.UserId.ToString());

			if (user == null)
			{
				return ResponseDto<AccessTokenDto>.Fail("Kullanıcı Bulunamadı", 404, false);
			}

			if (existRefreshToken.Expiration < DateTimeOffset.UtcNow)
			{
				return ResponseDto<AccessTokenDto>.Fail("Lütfen Tekrar Giriş Yapınız.", 404, true);
			}

			var tokenDto = await tokenService.CreateTokenAsync(user);
			existRefreshToken.Code = tokenDto.RefreshToken;
			existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

			userRefreshTokenRepository.Update(existRefreshToken);
			await SaveChangesAsync();
			return ResponseDto<AccessTokenDto>.Success(tokenDto, 200);
		}

		public async Task<ResponseDto<NoContentDto>> RevokeRefreshTokenAsync(RefreshTokenDto refreshToken)
		{
			var existRefreshToken = await userRefreshTokenRepository.GetAll().Where(x => x.Code == refreshToken.Token).SingleOrDefaultAsync();

			if (existRefreshToken == null)
			{
				return ResponseDto<NoContentDto>.Fail("Refresh Token Bulunamadı", 404, false);
			}

			userRefreshTokenRepository.Delete(existRefreshToken);
			await SaveChangesAsync();
			return ResponseDto<NoContentDto>.Success(200);
		}
	}
}
