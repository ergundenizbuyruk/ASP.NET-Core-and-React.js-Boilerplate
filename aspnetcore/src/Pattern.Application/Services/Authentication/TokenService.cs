using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pattern.Application.Services.Authentication.Dtos;
using Pattern.Application.Services.Base;
using Pattern.Core.Authentication;
using Pattern.Core.Entites.Authentication;
using Pattern.Core.Options;
using Pattern.Persistence.Repositories;
using Pattern.Persistence.UnitOfWork;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Pattern.Application.Services.Authentication
{
    public class TokenService : ApplicationService, ITokenService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly CustomTokenOption tokenOption;

        public TokenService(
            IUnitOfWork unitOfWork,
            IMapper objectMapper,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IOptions<CustomTokenOption> tokenOption
            ) : base(unitOfWork, objectMapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.tokenOption = tokenOption.Value;
        }

        public string CreateRefreshToken()
        {
            var numberByte = new byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }

        private async Task<IEnumerable<Claim>> GetClaims(User user)
        {
            var userClaimList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await userManager.GetRolesAsync(user);
            //userClaimList.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

            if (!roles.Any())
            {
                return userClaimList;
            }

            var permissions = await roleManager.Roles
                .Include(p => p.Permissions)
                .Where(p => roles.Contains(p.Name))
                .SelectMany(p => p.Permissions)
                .Distinct()
                .ToListAsync();

            userClaimList.AddRange(permissions.Select(x => new Claim(CustomClaims.Permissions, x.Name)));
            return userClaimList;
        }
        public async Task<AccessTokenDto> CreateTokenAsync(User userApp)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(tokenOption.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(tokenOption.RefreshTokenExpiration);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOption.SecurityKey));

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: tokenOption.Issuer,
                audience: tokenOption.Audience,
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
