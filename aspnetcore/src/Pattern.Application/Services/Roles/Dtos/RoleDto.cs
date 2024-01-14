using Pattern.Application.Services.Base.Dtos;

namespace Pattern.Application.Services.Roles.Dtos
{
    public class RoleDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }
}
