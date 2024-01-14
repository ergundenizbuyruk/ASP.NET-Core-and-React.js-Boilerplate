using Microsoft.AspNetCore.Identity;

namespace Pattern.Core.Entites.Authentication
{
    public class Role : IdentityRole<Guid>
    {
        public List<Permission> Permissions { get; set; }
    }
}
