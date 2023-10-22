using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pattern.Application.Services.Authentication.Dtos;
using Pattern.Core.Entites.Authentication;
using Pattern.Core.Responses;
using Pattern.Persistence.Repositories;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Authentication
{
	public class AuthenticationService : BaseService, IAuthenticationService
	{
		private readonly ITokenService _tokenService;
		private readonly UserManager<User> _userManager;
		private readonly IRepository<UserRefreshToken, int> _userRefreshTokenRepository;

		public AuthenticationService(IUnitOfWork unitOfWork, IMapper objectMapper, ITokenService tokenService, UserManager<User> userManager,
			IRepository<UserRefreshToken, int> userRefreshTokenRepository) : base(unitOfWork, objectMapper)
		{
			_tokenService = tokenService;
			_userManager = userManager;
			_userRefreshTokenRepository = userRefreshTokenRepository;
		}

		public async Task<ResponseDto<AccessTokenDto>> CreateTokenAsync(LoginDto loginDto)
		{
			var user = await _userManager.FindByEmailAsync(loginDto.Email);

			if (user == null)
			{
				return ResponseDto<AccessTokenDto>.Fail("E-posta veya Parola hatalı", 400);
			}

			if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
			{
				return ResponseDto<AccessTokenDto>.Fail("E-posta veya Parola hatalı", 400);
			}

			var token = await _tokenService.CreateTokenAsync(user);
			var userRefreshToken = await _userRefreshTokenRepository.GetAll().Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

			if (userRefreshToken == null)
			{
				await _userRefreshTokenRepository.CreateAsync(new UserRefreshToken
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
				_userRefreshTokenRepository.Update(userRefreshToken);
			}

			await SaveChangesAsync();
			return ResponseDto<AccessTokenDto>.Success(token, 200);
		}

		public async Task<ResponseDto<AccessTokenDto>> CreateTokenByRefreshTokenAsync(RefreshTokenDto refreshToken)
		{
			var existRefreshToken = await _userRefreshTokenRepository.GetAll()
				.Where(x => x.Code == refreshToken.Token).SingleOrDefaultAsync();

			if (existRefreshToken == null)
			{
				return ResponseDto<AccessTokenDto>.Fail("Refresh Token Hatalı", 404);
			}

			var user = await _userManager.FindByIdAsync(existRefreshToken.UserId.ToString());

			if (user == null)
			{
				return ResponseDto<AccessTokenDto>.Fail("Kullanıcı Bulunamadı", 404);
			}

			var tokenDto = await _tokenService.CreateTokenAsync(user);
			existRefreshToken.Code = tokenDto.RefreshToken;
			existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;
			await SaveChangesAsync();
			return ResponseDto<AccessTokenDto>.Success(tokenDto, 200);
		}

		public async Task<ResponseDto<NoContentDto>> RevokeRefreshTokenAsync(RefreshTokenDto refreshToken)
		{
			var existRefreshToken = await _userRefreshTokenRepository.GetAll().Where(x => x.Code == refreshToken.Token).SingleOrDefaultAsync();

			if (existRefreshToken == null)
			{
				return ResponseDto<NoContentDto>.Fail("Refresh Token Bulunamadı", 404);
			}

			_userRefreshTokenRepository.Delete(existRefreshToken);
			await SaveChangesAsync();
			return ResponseDto<NoContentDto>.Success(200);
		}
	}
}
