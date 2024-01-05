using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pattern.Application.Services.Emails;
using Pattern.Application.Services.Users.Dtos;
using Pattern.Core.Entites.Authentication;
using Pattern.Core.Responses;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Users
{
	public class UserService : BaseService, IUserService
	{
		private readonly UserManager<User> userManager;
		private readonly IEmailService emailService;

		public UserService(IUnitOfWork unitOfWork, IMapper objectMapper, UserManager<User> userManager,
			IEmailService emailService) : base(unitOfWork, objectMapper)
		{
			this.userManager = userManager;
			this.emailService = emailService;
		}

		public async Task<ResponseDto<UserDto>> GetUserAsync(Guid userId)
		{
			var user = await userManager.FindByIdAsync(userId.ToString());
			if (user == null)
			{
				return ResponseDto<UserDto>.Fail("Kullanıcı Bulunamadı", 404);
			}
			return ResponseDto<UserDto>.Success(ObjectMapper.Map<UserDto>(user), 200);
		}

		public async Task<ResponseDto<List<UserDto>>> GetAllUserAsync()
		{
			var userList = await userManager.Users.ToListAsync();
			return ResponseDto<List<UserDto>>.Success(ObjectMapper.Map<List<UserDto>>(userList), 200);
		}

		public async Task<ResponseDto<UserDto>> AddUserAsync(CreateUserDto userDto)
		{
			var user = ObjectMapper.Map<User>(userDto);

			var result = await userManager.CreateAsync(user, userDto.Password);

			if (!result.Succeeded)
			{
				return ResponseDto<UserDto>.Fail(new ErrorDto(result.Errors.Select(p => p.Description).ToList()), 400);
			}

			var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
			await emailService.SendEmailConfirmEmailAsync(user.Email, user.Id, token);

			return ResponseDto<UserDto>.Success(ObjectMapper.Map<UserDto>(user), 201);
		}

		public async Task<ResponseDto<UserDto>> UpdateUserAsync(UpdateUserDto updateUser)
		{
			var user = await userManager.FindByIdAsync(updateUser.Id.ToString());
			if (user == null)
			{
				return ResponseDto<UserDto>.Fail("Kullanıcı Bulunamadı", 404);
			}

			user.FirstName = updateUser.FirstName;
			user.LastName = updateUser.LastName;
			user.BirthDate = updateUser.BirthDate;
			user.PhoneNumber = updateUser.PhoneNumber;

			var result = await userManager.UpdateAsync(user);
			if (!result.Succeeded)
			{
				return ResponseDto<UserDto>.Fail(new ErrorDto(result.Errors.Select(p => p.Description).ToList()), 400);
			}

			return ResponseDto<UserDto>.Success(ObjectMapper.Map<UserDto>(user), 200);
		}

		public async Task<ResponseDto<NoContentDto>> DeleteUserAsync(Guid userId)
		{
			var user = await userManager.FindByIdAsync(userId.ToString());
			if (user == null)
			{
				return ResponseDto<NoContentDto>.Fail("Kullanıcı Bulunamadı.", 400);
			}

			var result = await userManager.DeleteAsync(user);
			if (!result.Succeeded)
			{
				return ResponseDto<NoContentDto>.Fail(new ErrorDto(result.Errors.Select(p => p.Description).ToList()), 400);
			}

			return ResponseDto<NoContentDto>.Success(200);
		}

		public async Task<ResponseDto<NoContentDto>> GeneratePasswordResetTokenAndSendEmailAsync(string userEmail)
		{
			var userFromDb = await userManager.FindByEmailAsync(userEmail);

			if (userFromDb == null)
			{
				return ResponseDto<NoContentDto>.Fail("Kullanıcı bulunamadı.", 404);
			}

			var token = await userManager.GeneratePasswordResetTokenAsync(userFromDb);
			await emailService.SendPasswordResetEmailAsync(userFromDb.Email, userFromDb.Id, token);

			return ResponseDto<NoContentDto>.Success(200);
		}

		public async Task<ResponseDto<NoContentDto>> GenerateChangeEmailTokenAndSendEmailAsync(Guid userId, string newEmail)
		{
			var userFromDb = await userManager.FindByIdAsync(userId.ToString());

			if (userFromDb == null)
			{
				return ResponseDto<NoContentDto>.Fail("Kullanıcı bulunamadı.", 404);
			}

			string token = await userManager.GenerateChangeEmailTokenAsync(userFromDb, newEmail);
			await emailService.SendChangeEmailAsync(userFromDb.Email, newEmail, token);

			return ResponseDto<NoContentDto>.Success(200);
		}

		public async Task<ResponseDto<NoContentDto>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
		{
			var userFromDb = await userManager.FindByIdAsync(resetPasswordDto.UserId.ToString());

			if (userFromDb == null)
			{
				return ResponseDto<NoContentDto>.Fail("Kullanıcı bulunamadı.", 404);
			}

			var result = await userManager.ResetPasswordAsync(userFromDb, resetPasswordDto.Token, resetPasswordDto.NewPassword);

			if (!result.Succeeded)
			{
				return ResponseDto<NoContentDto>.Fail(new ErrorDto(result.Errors.Select(x => x.Description).ToList()), 400);
			}

			return ResponseDto<NoContentDto>.Success(200);
		}

		public async Task<ResponseDto<NoContentDto>> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto)
		{
			var user = await userManager.FindByIdAsync(confirmEmailDto.UserId.ToString());

			if (user == null)
			{
				return ResponseDto<NoContentDto>.Fail("Kullanıcı Bulunamadı.", 404);
			}

			var result = await userManager.ConfirmEmailAsync(user, confirmEmailDto.Token);

			if (!result.Succeeded)
			{
				return ResponseDto<NoContentDto>
					.Fail(new ErrorDto(result.Errors.Select(p => p.Description).ToList()), 400);
			}

			return ResponseDto<NoContentDto>.Success(200);
		}

		public async Task<ResponseDto<NoContentDto>> ConfirmNewEmailAsync(string oldEmail, string newEmail, string token)
		{
			var user = await userManager.FindByEmailAsync(oldEmail);

			if (user == null)
			{
				return ResponseDto<NoContentDto>.Fail("Bu Email'e ait kullanıcı bulunamadı.", 404);
			}

			var result = await userManager.ChangeEmailAsync(user, newEmail, token);

			if (!result.Succeeded)
			{
				return ResponseDto<NoContentDto>.Fail(new ErrorDto(result.Errors.Select(x => x.Description).ToList()), 400);
			}

			return ResponseDto<NoContentDto>.Success(200);
		}

		public async Task<ResponseDto<NoContentDto>> ChangePasswordAsync(Guid userId, ChangePasswordDto password)
		{
			var userFromDb = await userManager.FindByIdAsync(userId.ToString());

			if (userFromDb == null)
			{
				return ResponseDto<NoContentDto>.Fail("Kullanıcı bulunamadı.", 404);
			}

			var result = await userManager.ChangePasswordAsync(userFromDb, password.CurrentPassword, password.NewPassword);

			if (!result.Succeeded)
			{
				return ResponseDto<NoContentDto>.Fail(new ErrorDto(result.Errors.Select(x => x.Description).ToList()), 400);
			}

			return ResponseDto<NoContentDto>.Success(200);
		}
	}
}
