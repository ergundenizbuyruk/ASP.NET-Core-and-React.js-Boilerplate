using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pattern.Application.Services.Base;
using Pattern.Application.Services.Emails;
using Pattern.Application.Services.Users.Dtos;
using Pattern.Core;
using Pattern.Core.Entites.Authentication;
using Pattern.Core.Exceptions;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Users;

public class UserService(
    IUnitOfWork unitOfWork,
    IMapper objectMapper,
    UserManager<User> userManager,
    IEmailService emailService,
    IResourceLocalizer localizer)
    : ApplicationService(unitOfWork, objectMapper), IUserService
{
    private readonly IEmailService emailService = emailService;

    public async Task<UserDto> GetUserByIdAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            throw new NotFoundException(localizer.Localize("UserNotFound"));
        }

        return ObjectMapper.Map<UserDto>(user);
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var userList = await userManager.Users.ToListAsync();
        return ObjectMapper.Map<List<UserDto>>(userList);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto userDto)
    {
        var user = ObjectMapper.Map<User>(userDto);
        user.IsActive = true;
        user.UserName = userDto.Email;

        var result = await userManager.CreateAsync(user, userDto.Password);

        if (!result.Succeeded)
        {
            // TODO: message list parameter
            throw new BadRequestException(string.Join(';', result.Errors.Select(p => p.Description)));
        }

        //var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        //await emailService.SendEmailConfirmEmailAsync(user.Email, user.Id, HttpUtility.UrlEncode(token));

        return ObjectMapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateProfileAsync(UpdateProfileDto updateProfileDto, Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            throw new NotFoundException(localizer.Localize("UserNotFound"));
        }

        user.FirstName = updateProfileDto.FirstName;
        user.LastName = updateProfileDto.LastName;
        user.PhoneNumber = updateProfileDto.PhoneNumber;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new BadRequestException(string.Join(';', result.Errors.Select(p => p.Description)));
        }

        return ObjectMapper.Map<UserDto>(user);
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            throw new NotFoundException(localizer.Localize("UserNotFound"));
        }

        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            throw new BadRequestException(string.Join(';', result.Errors.Select(p => p.Description)));
        }
    }

    public async Task GeneratePasswordResetTokenAndSendEmailAsync(string userEmail)
    {
        var userFromDb = await userManager.FindByEmailAsync(userEmail);

        if (userFromDb == null)
        {
            throw new NotFoundException(localizer.Localize("UserNotFound"));
        }

        //var token = await userManager.GeneratePasswordResetTokenAsync(userFromDb);
        //await emailService.SendPasswordResetEmailAsync(userFromDb.Email, userFromDb.Id, HttpUtility.UrlEncode(token));
    }

    public async Task GenerateChangeEmailTokenAndSendEmailAsync(Guid userId,
        string newEmail)
    {
        var userFromDb = await userManager.FindByIdAsync(userId.ToString());

        if (userFromDb == null)
        {
            throw new NotFoundException(localizer.Localize("UserNotFound"));
        }

        //string token = await userManager.GenerateChangeEmailTokenAsync(userFromDb, newEmail);
        //await emailService.SendChangeEmailAsync(userFromDb.Email, newEmail, HttpUtility.UrlEncode(token));
    }

    public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var userFromDb = await userManager.FindByIdAsync(resetPasswordDto.UserId.ToString());

        if (userFromDb == null)
        {
            throw new NotFoundException(localizer.Localize("UserNotFound"));
        }

        var result =
            await userManager.ResetPasswordAsync(userFromDb, resetPasswordDto.Token, resetPasswordDto.NewPassword);

        if (!result.Succeeded)
        {
            throw new BadRequestException(string.Join(';', result.Errors.Select(p => p.Description)));
        }
    }

    public async Task ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto)
    {
        var user = await userManager.FindByIdAsync(confirmEmailDto.UserId.ToString());

        if (user == null)
        {
            throw new NotFoundException(localizer.Localize("UserNotFound"));
        }

        var result = await userManager.ConfirmEmailAsync(user, confirmEmailDto.Token);

        if (!result.Succeeded)
        {
            throw new BadRequestException(string.Join(';', result.Errors.Select(p => p.Description)));
        }
    }

    public async Task ConfirmNewEmailAsync(string oldEmail, string newEmail,
        string token)
    {
        var user = await userManager.FindByEmailAsync(oldEmail);

        if (user == null)
        {
            throw new NotFoundException(localizer.Localize("UserNotFound"));
        }

        var result = await userManager.ChangeEmailAsync(user, newEmail, token);

        if (!result.Succeeded)
        {
            throw new BadRequestException(string.Join(';', result.Errors.Select(p => p.Description)));
        }
    }

    public async Task ChangePasswordAsync(Guid userId, ChangePasswordDto password)
    {
        var userFromDb = await userManager.FindByIdAsync(userId.ToString());

        if (userFromDb == null)
        {
            throw new NotFoundException(localizer.Localize("UserNotFound"));
        }

        var result =
            await userManager.ChangePasswordAsync(userFromDb, password.CurrentPassword, password.NewPassword);

        if (!result.Succeeded)
        {
            throw new BadRequestException(string.Join(';', result.Errors.Select(p => p.Description)));
        }
    }
}