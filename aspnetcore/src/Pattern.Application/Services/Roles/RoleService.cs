using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pattern.Application.Services.Base;
using Pattern.Application.Services.Roles.Dtos;
using Pattern.Core;
using Pattern.Core.Entites.Authentication;
using Pattern.Core.Exceptions;
using Pattern.Persistence.Context;
using Pattern.Persistence.Repositories;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Roles;

public class RoleService(
    IUnitOfWork unitOfWork,
    IMapper objectMapper,
    RoleManager<Role> roleManager,
    IRepository<Permission, int> permissionRepository,
    ApplicationDbContext context,
    IResourceLocalizer localizer)
    : ApplicationService(unitOfWork, objectMapper), IRoleService
{
    public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto)
    {
        var role = new Role
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
        return ObjectMapper.Map<RoleDto>(role);
    }

    public async Task<RoleDto> UpdateRoleAsync(UpdateRoleDto updateRoleDto)
    {
        var role = await roleManager.Roles
            .Include(p => p.Permissions)
            .FirstOrDefaultAsync(p => p.Id == updateRoleDto.Id);

        if (role is null)
        {
            throw new NotFoundException(localizer.Localize("RoleNotFound"));
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

        await roleManager.UpdateAsync(role);
        await SaveChangesAsync();
        return ObjectMapper.Map<RoleDto>(role);
    }

    public async Task<RoleDto> GetRoleByIdAsync(Guid id)
    {
        var role = await roleManager.Roles
            .Include(p => p.Permissions)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (role is null)
        {
            throw new NotFoundException(localizer.Localize("RoleNotFound"));
        }

        return ObjectMapper.Map<RoleDto>(role);
    }

    public async Task<List<RoleDto>> GetRolesAsync()
    {
        var roles = await roleManager.Roles
            .Include(p => p.Permissions)
            .ToListAsync();

        return ObjectMapper.Map<List<RoleDto>>(roles);
    }

    public async Task DeleteRoleAsync(Guid id)
    {
        var role = await roleManager.FindByIdAsync(id.ToString());

        if (role == null)
        {
            throw new NotFoundException(localizer.Localize("RoleNotFound"));
        }

        await roleManager.DeleteAsync(role);
        await SaveChangesAsync();
    }

    public async Task<List<PermissionDto>> GetAllPermissionsAsync()
    {
        var permissions = await permissionRepository.GetAll().ToListAsync();
        return ObjectMapper.Map<List<PermissionDto>>(permissions);
    }
}