using Pattern.Application.Services.Base;
using Pattern.Application.Services.Roles.Dtos;
using Pattern.Core.Responses;

namespace Pattern.Application.Services.Roles
{
	public interface IRoleService : IApplicationService
	{
		public Task<ResponseDto<RoleDto>> CreateRoleAsync(CreateRoleDto createRoleDto);
		public Task<ResponseDto<RoleDto>> UpdateRoleAsync(UpdateRoleDto updateRoleDto);
		public Task<ResponseDto<RoleDto>> GetRoleByIdAsync(Guid id);
		public Task<ResponseDto<List<RoleDto>>> GetRolesAsync();
		public Task<ResponseDto<NoContentDto>> DeleteRoleAsync(Guid id);
		public Task<ResponseDto<List<PermissionDto>>> GetAllPermissionsAsync();

	}
}
