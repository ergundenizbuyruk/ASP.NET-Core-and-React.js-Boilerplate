using Microsoft.AspNetCore.Authorization;

namespace Pattern.Core.Authentication
{
    public class PermissionRequirement(string[] permissions) : IAuthorizationRequirement
    {
        public string[] Permissions { get; } = permissions;
    }
}