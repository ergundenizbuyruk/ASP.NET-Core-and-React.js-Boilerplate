using Microsoft.AspNetCore.Authorization;

namespace Pattern.Core.Authentication
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var userPermissions = context.User.Claims
                .Where(c => c.Type == CustomClaims.Permissions)
                .Select(c => c.Value)
                .ToArray();
            
            foreach (var requiredPermission in requirement.Permissions)
            {
                if (!userPermissions.Contains(requiredPermission))
                {
                    return Task.CompletedTask;
                }
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}