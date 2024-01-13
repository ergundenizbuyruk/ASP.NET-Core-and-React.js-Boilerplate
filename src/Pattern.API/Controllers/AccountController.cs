﻿using Microsoft.AspNetCore.Mvc;
using Pattern.Application.Services.Users;
using Pattern.Application.Services.Users.Dtos;
using Pattern.Core.Attributes;
using Pattern.Core.Enums;
using System.Security.Claims;

namespace Pattern.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : BaseController
    {

        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(CreateUserDto createUserDto)
        {
            var result = await userService.CreateUserAsync(createUserDto);
            return ActionResultInstance(result);
        }

        [HttpGet]
        [HasPermissions(Permission.AccountDefault)]
        public async Task<IActionResult> GetUserInformation()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await userService.GetUserByIdAsync(userId);
            return ActionResultInstance(result);
        }

        [HttpPut]
        [HasPermissions(Permission.AccountUpdate)]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto updateProfileDto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await userService.UpdateProfileAsync(updateProfileDto, userId);
            return ActionResultInstance(result);
        }

        [HttpDelete]
        [HasPermissions(Permission.AccountDelete)]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await userService.DeleteUserAsync(userId);
            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> SendPasswordResetEmail(PasswordResetTokenDto passwordResetTokenDto)
        {
            var result = await userService.GeneratePasswordResetTokenAndSendEmailAsync(passwordResetTokenDto.Email);
            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var result = await userService.ResetPasswordAsync(resetPasswordDto);
            return ActionResultInstance(result);
        }

        [HttpPost]
        [HasPermissions(Permission.EmailChange)]
        public async Task<IActionResult> SendEmailChangeEmail(SendEmailChangeEmailDto sendEmailChangeEmailDto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await userService.GenerateChangeEmailTokenAndSendEmailAsync(userId, sendEmailChangeEmailDto.NewEmail);
            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {
            var result = await userService.ConfirmEmailAsync(confirmEmailDto);
            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmNewEmail(ConfirmNewEmailDto confirmNewEmailDto)
        {
            var result = await userService.ConfirmNewEmailAsync(confirmNewEmailDto.OldEmail, confirmNewEmailDto.NewEmail, confirmNewEmailDto.Token);
            return ActionResultInstance(result);
        }

        [HttpPost]
        [HasPermissions(Permission.ChangePassword)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await userService.ChangePasswordAsync(userId, changePasswordDto);
            return ActionResultInstance(result);
        }
    }
}
