using Pattern.Application.Services.Base;
using Pattern.Application.Services.Users.Dtos;
using Pattern.Core.Responses;

namespace Pattern.Application.Services.Users
{
	public interface IUserService : IApplicationService
	{
		public Task<ResponseDto<UserDto>> GetUserAsync(Guid userId);
		public Task<ResponseDto<List<UserDto>>> GetAllUserAsync();
		public Task<ResponseDto<UserDto>> AddUserAsync(CreateUserDto userDto);
		public Task<ResponseDto<UserDto>> UpdateProfileAsync(UpdateProfileDto updateProfileDto, Guid userId);
		public Task<ResponseDto<NoContentDto>> DeleteUserAsync(Guid userId);
		public Task<ResponseDto<NoContentDto>> GeneratePasswordResetTokenAndSendEmailAsync(string userEmail);
		public Task<ResponseDto<NoContentDto>> GenerateChangeEmailTokenAndSendEmailAsync(Guid userId, string newEmail);
		public Task<ResponseDto<NoContentDto>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
		public Task<ResponseDto<NoContentDto>> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto);
		public Task<ResponseDto<NoContentDto>> ConfirmNewEmailAsync(string oldEmail, string newEmail, string token);
		public Task<ResponseDto<NoContentDto>> ChangePasswordAsync(Guid userId, ChangePasswordDto password);
	}
}
