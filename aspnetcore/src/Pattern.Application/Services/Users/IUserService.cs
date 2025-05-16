using Pattern.Application.Services.Base;
using Pattern.Application.Services.Users.Dtos;
using Pattern.Core.Responses;

namespace Pattern.Application.Services.Users;

public interface IUserService : IApplicationService
{
    public Task<UserDto> GetUserByIdAsync(Guid userId);
    public Task<List<UserDto>> GetUsersAsync();
    public Task<UserDto> CreateUserAsync(CreateUserDto userDto);
    public Task<UserDto> UpdateProfileAsync(UpdateProfileDto updateProfileDto, Guid userId);
    public Task DeleteUserAsync(Guid userId);
    public Task GeneratePasswordResetTokenAndSendEmailAsync(string userEmail);
    public Task GenerateChangeEmailTokenAndSendEmailAsync(Guid userId, string newEmail);
    public Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    public Task ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto);
    public Task ConfirmNewEmailAsync(string oldEmail, string newEmail, string token);
    public Task ChangePasswordAsync(Guid userId, ChangePasswordDto password);
}