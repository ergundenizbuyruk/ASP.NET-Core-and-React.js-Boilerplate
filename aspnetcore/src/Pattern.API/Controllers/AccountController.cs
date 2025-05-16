using Microsoft.AspNetCore.Mvc;
using Pattern.Application.Services.Users;
using Pattern.Application.Services.Users.Dtos;
using Pattern.Core.Attributes;
using Pattern.Core.Enums;
using System.Security.Claims;

namespace Pattern.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController(IUserService userService) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Register(CreateUserDto createUserDto)
    {
        var result = await userService.CreateUserAsync(createUserDto);
        return Success(result);
    }

    [HttpGet]
    [HasPermissions(Permission.AccountDefault)]
    public async Task<IActionResult> GetUserInformation()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await userService.GetUserByIdAsync(userId);
        return Success(result);
    }

    [HttpPut]
    [HasPermissions(Permission.AccountUpdate)]
    public async Task<IActionResult> UpdateProfile(UpdateProfileDto updateProfileDto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await userService.UpdateProfileAsync(updateProfileDto, userId);
        return Success(result);
    }

    [HttpDelete]
    [HasPermissions(Permission.AccountDelete)]
    public async Task<IActionResult> DeleteAccount()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await userService.DeleteUserAsync(userId);
        return Success();
    }

    [HttpPost]
    public async Task<IActionResult> SendPasswordResetEmail(PasswordResetTokenDto passwordResetTokenDto)
    {
        await userService.GeneratePasswordResetTokenAndSendEmailAsync(passwordResetTokenDto.Email);
        return Success();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        await userService.ResetPasswordAsync(resetPasswordDto);
        return Success();
    }

    [HttpPost]
    [HasPermissions(Permission.EmailChange)]
    public async Task<IActionResult> SendEmailChangeEmail(SendEmailChangeEmailDto sendEmailChangeEmailDto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await userService.GenerateChangeEmailTokenAndSendEmailAsync(userId, sendEmailChangeEmailDto.NewEmail);
        return Success();
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
    {
        await userService.ConfirmEmailAsync(confirmEmailDto);
        return Success();
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmNewEmail(ConfirmNewEmailDto confirmNewEmailDto)
    {
        await userService.ConfirmNewEmailAsync(confirmNewEmailDto.OldEmail, confirmNewEmailDto.NewEmail,
            confirmNewEmailDto.Token);
        return Success();
    }

    [HttpPost]
    [HasPermissions(Permission.ChangePassword)]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await userService.ChangePasswordAsync(userId, changePasswordDto);
        return Success();
    }
}