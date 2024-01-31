using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pattern.Application.Services.Base;
using Pattern.Application.Services.Roles.Dtos;
using Pattern.Core.Entites.Authentication;
using Pattern.Core.Responses;
using Pattern.Persistence.Repositories;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Roles
{
	public class RoleService : ApplicationService, IRoleService
	{
		public UserManager<User> userManager { get; set; }
		public RoleManager<Role> roleManager { get; set; }
		public IRepository<Permission, int> permissionRepository;

		public RoleService(
			IUnitOfWork unitOfWork,
			IMapper objectMapper,
			UserManager<User> userManager,
			RoleManager<Role> roleManager,
			IRepository<Permission, int> permissionRepository) :
			base(unitOfWork, objectMapper)
		{
			this.userManager = userManager;
			this.roleManager = roleManager;
			this.permissionRepository = permissionRepository;
		}

		public async Task<ResponseDto<RoleDto>> CreateRoleAsync(CreateRoleDto createRoleDto)
		{
			var role = new Role()
			{
				Name = createRoleDto.Name,
				Permissions = new()
			};

			var permissions = await permissionRepository.GetAll()
				.Where(p => createRoleDto.PermissionIds.Contains(p.Id))
				.ToListAsync();

			foreach (var permission in permissions)
			{
				role.Permissions.Add(permission);
			}

			await roleManager.CreateAsync(role);
			await SaveChangesAsync();

			var roleDto = ObjectMapper.Map<Role, RoleDto>(role);
			return ResponseDto<RoleDto>.Success(roleDto, 201);
		}

		public async Task<ResponseDto<RoleDto>> UpdateRoleAsync(UpdateRoleDto updateRoleDto)
		{
			var role = await roleManager.Roles
					.Include(p => p.Permissions)
					.FirstOrDefaultAsync(p => p.Id == updateRoleDto.Id);

			if (role == null)
			{
				return ResponseDto<RoleDto>.Fail("Role not found.", 404);
			}

			role.Name = updateRoleDto.Name;

			var permissions = await permissionRepository.GetAll()
			   .Where(rp => updateRoleDto.PermissionIds.Contains(rp.Id))
			   .ToListAsync();

			role.Permissions.RemoveAll(p => !updateRoleDto.PermissionIds.Contains(p.Id));

			var newPermission = permissions.Where(p => !role.Permissions.Contains(p));
			role.Permissions.AddRange(newPermission);

			var result = await roleManager.UpdateAsync(role);
			await SaveChangesAsync();

			var roleDto = ObjectMapper.Map<Role, RoleDto>(role);
			return ResponseDto<RoleDto>.Success(roleDto, 200);
		}

		public async Task<ResponseDto<RoleDto>> GetRoleByIdAsync(Guid id)
		{
			var role = await roleManager.Roles
				.Include(p => p.Permissions)
				.FirstOrDefaultAsync(p => p.Id == id);

			if (role == null)
			{
				return ResponseDto<RoleDto>.Fail("Role not found.", 404);
			}

			var roleDto = ObjectMapper.Map<Role, RoleDto>(role);
			return ResponseDto<RoleDto>.Success(roleDto, 200);
		}

		public async Task<ResponseDto<List<RoleDto>>> GetRolesAsync()
		{
			var roles = await roleManager.Roles
				.Include(p => p.Permissions)
				.ToListAsync();

			var roleDtos = ObjectMapper.Map<List<Role>, List<RoleDto>>(roles);
			return ResponseDto<List<RoleDto>>.Success(roleDtos, 200);
		}

		public async Task<ResponseDto<NoContentDto>> DeleteRoleAsync(Guid id)
		{
			var role = await roleManager.FindByIdAsync(id.ToString());

			if (role == null)
			{
				return ResponseDto<NoContentDto>.Fail("Role not found.", 404);
			}

			await roleManager.DeleteAsync(role);
			await SaveChangesAsync();
			return ResponseDto<NoContentDto>.Success(200);
		}

		public async Task<ResponseDto<List<PermissionDto>>> GetAllPermissionsAsync()
		{
			var permissions = await permissionRepository.GetAll().ToListAsync();
			var permissionDtos = ObjectMapper.Map<List<Permission>, List<PermissionDto>>(permissions);
			return ResponseDto<List<PermissionDto>>.Success(permissionDtos, 200);
		}
	}
}
