namespace Pattern.Application.Services.Roles.Dtos
{
    public class CreateRoleDto
    {
        public string Name { get; set; }
        public List<int> PermissionIds { get; set; }
    }
}
