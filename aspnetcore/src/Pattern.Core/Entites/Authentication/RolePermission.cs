namespace Pattern.Core.Entites.Authentication
{
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
