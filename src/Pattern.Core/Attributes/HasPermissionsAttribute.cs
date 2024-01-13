using Microsoft.AspNetCore.Authorization;
using Pattern.Core.Enums;

namespace Pattern.Core.Attributes
{
    public sealed class HasPermissionsAttribute : AuthorizeAttribute
    {
        public Permission[] Permissions { get; set; }

        public HasPermissionsAttribute(params Permission[] permissions)
            : base(policy: string.Join(",", permissions))
        {
            Permissions = permissions;
        }
    }
}
