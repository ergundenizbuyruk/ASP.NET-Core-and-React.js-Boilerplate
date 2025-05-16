using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Pattern.Core.Authentication
{
    public class PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        : DefaultAuthorizationPolicyProvider(options)
    {
        public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(policyName.Split(",")))
                .Build();

            return Task.FromResult(policy)!;
        }
    }
}