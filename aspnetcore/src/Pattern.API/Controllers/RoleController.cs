using Microsoft.AspNetCore.Mvc;
using Pattern.Application.Services.Roles;
using Pattern.Application.Services.Roles.Dtos;
using Pattern.Core.Attributes;
using Pattern.Core.Enums;

namespace Pattern.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseController
    {
        private readonly IRoleService roleService;
        private readonly IResourceLocalizer resourceLocalizer;

        public RoleController(IRoleService roleService, IResourceLocalizer resourceLocalizer)
        {
            this.roleService = roleService;
            this.resourceLocalizer = resourceLocalizer;
        }

        [HttpGet("{id:guid}")]
        [HasPermissions(Permission.RoleDefault)]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await roleService.GetRoleByIdAsync(id);
            return ActionResultInstance(result);
        }

        [HttpGet]
        [HasPermissions(Permission.RoleDefault)]
        public async Task<IActionResult> GetAll()
        {
            var result = await roleService.GetRolesAsync();
            return ActionResultInstance(result);
        }

        [HttpPost]
        [HasPermissions(Permission.RoleCreate)]
        public async Task<IActionResult> Create(CreateRoleDto createRoleDto)
        {
            var result = await roleService.CreateRoleAsync(createRoleDto);
            return ActionResultInstance(result);
        }

        [HttpPut]
        [HasPermissions(Permission.RoleUpdate)]
        public async Task<IActionResult> Update(UpdateRoleDto updateRoleDto)
        {
            var result = await roleService.UpdateRoleAsync(updateRoleDto);
            return ActionResultInstance(result);
        }

        [HttpDelete("{id:guid}")]
        [HasPermissions(Permission.RoleDelete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await roleService.DeleteRoleAsync(id);
            return ActionResultInstance(result);
        }

        [HttpGet("permissions")]
        [HasPermissions]
        public async Task<IActionResult> GetAllPermissions()
        {
            var result = await roleService.GetAllPermissionsAsync();
            return ActionResultInstance(result);
        }
    }
}