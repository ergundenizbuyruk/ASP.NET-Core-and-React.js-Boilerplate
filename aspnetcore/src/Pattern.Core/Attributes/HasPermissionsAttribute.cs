using Microsoft.AspNetCore.Authorization;
using Pattern.Core.Enums;

namespace Pattern.Core.Attributes
{
    public sealed class HasPermissionsAttribute(params Permission[] permissions)
        : AuthorizeAttribute(policy: string.Join(",", permissions.Select(p => (int)p)))
    {
        public Permission[] Permissions { get; set; } = permissions;
    }
}