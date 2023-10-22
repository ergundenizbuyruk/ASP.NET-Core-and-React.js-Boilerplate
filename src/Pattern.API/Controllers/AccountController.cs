using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pattern.Application.Services.Users;
using Pattern.Application.Services.Users.Dtos;
using System.Security.Claims;

namespace Pattern.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : BaseController
    {

        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> SendPasswordResetEmail(PasswordResetTokenDto passwordResetTokenDto)
        {
            var result = await _userService.GeneratePasswordResetTokenAndSendEmailAsync(passwordResetTokenDto);
            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var result = await _userService.ResetPasswordAsync(resetPasswordDto);
            return ActionResultInstance(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendEmailChangeEmail(SendEmailChangeEmailDto sendEmailChangeEmailDto)
        {
            Guid userId = Guid.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _userService.GenerateChangeEmailTokenAndSendEmailAsync(userId, sendEmailChangeEmailDto.NewEmail);
            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmNewEmail(ConfirmNewEmailDto confirmNewEmailDto)
        {
            var result = await _userService.ConfirmNewEmailAsync(confirmNewEmailDto.OldEmail, confirmNewEmailDto.NewEmail, confirmNewEmailDto.Token);
            return ActionResultInstance(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            Guid userId = Guid.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _userService.ChangePasswordAsync(userId, changePasswordDto);
            return ActionResultInstance(result);
        }
    }
}
