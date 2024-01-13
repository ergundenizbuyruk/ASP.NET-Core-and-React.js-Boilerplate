using Pattern.Core.Entites.BaseEntity;

namespace Pattern.Core.Entites.Authentication
{
    public class Permission : Entity
    {
        public string Name { get; set; }
        public List<Role> Roles { get; set; }
    }
}
