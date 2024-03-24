using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pattern.Application.Services.Base;
using Pattern.Application.Services.Roles.Dtos;
using Pattern.Core.Entites.Authentication;
using Pattern.Core.Responses;
using Pattern.Persistence.Context;
using Pattern.Persistence.Repositories;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Roles
{
	public class RoleService : ApplicationService, IRoleService
	{
		private UserManager<User> userManager { get; set; }

		private RoleManager<Role> roleManager { get; set; }

		private IRepository<Permission, int> permissionRepository;

		private readonly ApplicationDbContext context;


		public RoleService(
			IUnitOfWork unitOfWork,
			IMapper objectMapper,
			UserManager<User> userManager,
			RoleManager<Role> roleManager,
			IRepository<Permission, int> permissionRepository,
			ApplicationDbContext context) :
			base(unitOfWork, objectMapper)
		{
			this.userManager = userManager;
			this.roleManager = roleManager;
			this.permissionRepository = permissionRepository;
			this.context = context;
		}

		public async Task<ResponseDto<RoleDto>> CreateRoleAsync(CreateRoleDto createRoleDto)
		{
			try
			{
				var role = new Role()
				{
					Id = Guid.NewGuid(),
					Name = createRoleDto.Name
				};

				var permissions = await context.Permissions
					.Where(p => createRoleDto.PermissionIds.Contains(p.Id))
					.ToListAsync();

				foreach (var permission in permissions)
				{
					await context.RolePermissions.AddAsync(new RolePermission()
					{
						RoleId = role.Id,
						PermissionId = permission.Id
					});
				}

				await roleManager.CreateAsync(role);
				await SaveChangesAsync();

				var roleDto = ObjectMapper.Map<Role, RoleDto>(role);
				return ResponseDto<RoleDto>.Success(roleDto, 201);
			}
			catch (Exception e)
			{
				var a = e.Message;
				throw;
			}

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

			var rolePermissions = await context.RolePermissions
			   .Where(rp => rp.RoleId == role.Id)
			   .ToListAsync();

			context.RolePermissions.RemoveRange(rolePermissions);

			var permissions = await context.Permissions
					.Where(p => updateRoleDto.PermissionIds.Contains(p.Id))
					.ToListAsync();

			foreach (var permission in permissions)
			{
				await context.RolePermissions.AddAsync(new RolePermission()
				{
					RoleId = role.Id,
					PermissionId = permission.Id
				});
			}

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
