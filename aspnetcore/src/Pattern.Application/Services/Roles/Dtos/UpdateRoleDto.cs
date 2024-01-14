using Pattern.Application.Services.Base.Dtos;

namespace Pattern.Application.Services.Roles.Dtos
{
    public class UpdateRoleDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public List<int> PermissionIds { get; set; }
    }
}
