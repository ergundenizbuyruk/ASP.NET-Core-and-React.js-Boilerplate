using System.Globalization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Pattern.Core.Providers;

public class NumericTokenProvider<TUser>(
    IDataProtectionProvider dataProtectionProvider,
    IOptions<NumericTokenProviderOptions> options,
    ILogger<DataProtectorTokenProvider<TUser>> logger)
    : DataProtectorTokenProvider<TUser>(dataProtectionProvider, options, logger)
    where TUser : class
{
    public override async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
        ArgumentNullException.ThrowIfNull(manager);

        var token = await manager.CreateSecurityTokenAsync(user);
        var modifier = await GetUserModifierAsync(purpose, manager, user);
        return Rfc6238AuthenticationService.GenerateCode(token, modifier).ToString("D6", CultureInfo.InvariantCulture);
    }

    public override async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
    {
        ArgumentNullException.ThrowIfNull(manager);

        int code;
        if (!int.TryParse(token, out code))
        {
            return false;
        }

        var securityToken = await manager.CreateSecurityTokenAsync(user);
        var modifier = await GetUserModifierAsync(purpose, manager, user);
        return securityToken != null && Rfc6238AuthenticationService.ValidateCode(securityToken, code, modifier);
    }

    protected virtual async Task<string> GetUserModifierAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
        ArgumentNullException.ThrowIfNull(manager);

        var userId = await manager.GetUserIdAsync(user);
        return "Totp:" + purpose + ":" + userId;
    }
}

public class NumericTokenProviderOptions
    : DataProtectionTokenProviderOptions
{
}