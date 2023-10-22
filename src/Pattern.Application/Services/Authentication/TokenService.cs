using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pattern.Application.Services.Authentication.Dtos;
using Pattern.Core.Entites.Authentication;
using Pattern.Core.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Pattern.Application.Services.Authentication
{
	public class TokenService : ITokenService
	{
		private readonly UserManager<User> _userManager;
		private readonly CustomTokenOption _tokenOption;

		public TokenService(UserManager<User> userManager, IOptions<CustomTokenOption> customTokenOption)
		{
			_userManager = userManager;
			_tokenOption = customTokenOption.Value;
		}
		public string CreateRefreshToken()
		{
			var numberByte = new byte[32];
			using var rnd = RandomNumberGenerator.Create();
			rnd.GetBytes(numberByte);
			return Convert.ToBase64String(numberByte);
		}

		private async Task<IEnumerable<Claim>> GetClaims(User userApp)
		{
			var userList = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, userApp.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Email, userApp.Email),
				new Claim(ClaimTypes.Name, userApp.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var roles = await _userManager.GetRolesAsync(userApp);
			userList.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));
			return userList;
		}
		public async Task<AccessTokenDto> CreateTokenAsync(User userApp)
		{
			var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
			var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.RefreshTokenExpiration);
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOption.SecurityKey));

			SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

			JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
				issuer: _tokenOption.Issuer,
				audience: _tokenOption.Audience,
				expires: accessTokenExpiration,
				 notBefore: DateTime.Now,
				 claims: await GetClaims(userApp),
				 signingCredentials: signingCredentials);

			var handler = new JwtSecurityTokenHandler();
			var token = handler.WriteToken(jwtSecurityToken);
			var tokenDto = new AccessTokenDto
			{
				AccessToken = token,
				RefreshToken = CreateRefreshToken(),
				AccessTokenExpiration = accessTokenExpiration,
				RefreshTokenExpiration = refreshTokenExpiration
			};

			return tokenDto;
		}
	}
}
