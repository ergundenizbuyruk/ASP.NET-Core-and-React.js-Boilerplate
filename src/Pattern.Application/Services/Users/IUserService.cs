using Pattern.Application.Services.Users.Dtos;
using Pattern.Core.Responses;

namespace Pattern.Application.Services.Users
{
    public interface IUserService
    {
        public Task<ResponseDto<UserDto>> GetUserAsync(Guid userId);
        public Task<ResponseDto<List<UserDto>>> GetAllUserAsync();
        public Task<ResponseDto<UserDto>> AddUserAsync(CreateUserDto user);
        public Task<ResponseDto<UserDto>> UpdateUserAsync(UpdateUserDto updateUser);
        public Task<ResponseDto<NoContentDto>> DeleteUserAsync(Guid userId);
        public Task<ResponseDto<NoContentDto>> GeneratePasswordResetTokenAndSendEmailAsync(PasswordResetTokenDto passwordResetTokenDto);
        public Task<ResponseDto<NoContentDto>> ResetPasswordAsync(ResetPasswordDto user);
        public Task<ResponseDto<NoContentDto>> GenerateChangeEmailTokenAndSendEmailAsync(Guid userId, string newEmail);
        public Task<ResponseDto<NoContentDto>> ConfirmNewEmailAsync(string oldEmail, string newEmail, string token);
        public Task<ResponseDto<NoContentDto>> ChangePasswordAsync(Guid userId, ChangePasswordDto password);
    }
}
