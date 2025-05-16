using Pattern.Application.Services.Base;
using Pattern.Application.Services.Roles.Dtos;
using Pattern.Core.Responses;

namespace Pattern.Application.Services.Roles
{
	public interface IRoleService : IApplicationService
	{
		public Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto);
		public Task<RoleDto> UpdateRoleAsync(UpdateRoleDto updateRoleDto);
		public Task<RoleDto> GetRoleByIdAsync(Guid id);
		public Task<List<RoleDto>> GetRolesAsync();
		public Task DeleteRoleAsync(Guid id);
		public Task<List<PermissionDto>> GetAllPermissionsAsync();

	}
}
