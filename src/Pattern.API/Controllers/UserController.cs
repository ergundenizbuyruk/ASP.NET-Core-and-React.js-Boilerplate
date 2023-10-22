using Microsoft.AspNetCore.Mvc;
using Pattern.Application.Services.Users;
using Pattern.Application.Services.Users.Dtos;

namespace Pattern.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var user = await _userService.GetUserAsync(userId);
            return ActionResultInstance(user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDto user)
        {
            var result = await _userService.UpdateUserAsync(user);
            return ActionResultInstance(result);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            return ActionResultInstance(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserDto()
        {
            var result = await _userService.GetAllUserAsync();
            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto login)
        {
            var result = await _userService.AddUserAsync(login);
            return ActionResultInstance(result);
        }
    }
}
