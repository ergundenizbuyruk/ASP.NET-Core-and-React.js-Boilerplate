using Microsoft.AspNetCore.Identity;
using Pattern.Core.Providers;

namespace Pattern.API.Extensions;

public static class CustomTokenProvider
{
    public static void AddCustomTokenProvider(this IServiceCollection services)
    {
        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromMinutes(5);
        });

        services.Configure<NumericTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromMinutes(5);
        });
    }
}