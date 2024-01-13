using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Pattern.Core.Authentication
{
    public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
        }

        override public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(policyName.Split(",")))
                .Build();

            return Task.FromResult(policy);
        }
    }
}
