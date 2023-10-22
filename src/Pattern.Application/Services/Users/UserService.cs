using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pattern.Application.Services.Emails;
using Pattern.Application.Services.Users.Dtos;
using Pattern.Core.Entites.Authentication;
using Pattern.Core.Responses;
using Pattern.Persistence.UnitOfWork;
using System.Web;

namespace Pattern.Application.Services.Users
{
    public class UserService : BaseService, IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly CustomAppOptions _appOptions;

        public UserService(IUnitOfWork unitOfWork, IMapper objectMapper, UserManager<User> userManager,
            IEmailSender emailSender, IOptions<CustomAppOptions> appOptions) : base(unitOfWork, objectMapper)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _appOptions = appOptions.Value;
        }

        public async Task<ResponseDto<UserDto>> GetUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return ResponseDto<UserDto>.Fail("Kullanıcı Bulunamadı", 404);
            }
            return ResponseDto<UserDto>.Success(ObjectMapper.Map<UserDto>(user), 200);
        }

        public async Task<ResponseDto<List<UserDto>>> GetAllUserAsync()
        {
            var userList = await _userManager.Users.ToListAsync();
            return ResponseDto<List<UserDto>>.Success(ObjectMapper.Map<List<UserDto>>(userList), 200);
        }

        public async Task<ResponseDto<UserDto>> AddUserAsync(CreateUserDto userDto)
        {
            var user = ObjectMapper.Map<User>(userDto);

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                return ResponseDto<UserDto>.Fail(new ErrorDto(result.Errors.Select(p => p.Description).ToList()), 400);
            }

            return ResponseDto<UserDto>.Success(ObjectMapper.Map<UserDto>(user), 200);
        }

        public async Task<ResponseDto<UserDto>> UpdateUserAsync(UpdateUserDto updateUser)
        {
            var user = await _userManager.FindByIdAsync(updateUser.Id.ToString());
            if (user == null)
            {
                return ResponseDto<UserDto>.Fail("Kullanıcı Bulunamadı", 404);
            }

            user.FirstName = updateUser.FirstName;
            user.LastName = updateUser.LastName;
            user.BirthDate = updateUser.BirthDate;
            user.PhoneNumber = updateUser.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return ResponseDto<UserDto>.Fail(new ErrorDto(result.Errors.Select(p => p.Description).ToList()), 400);
            }

            return ResponseDto<UserDto>.Success(ObjectMapper.Map<UserDto>(user), 200);
        }

        public async Task<ResponseDto<NoContentDto>> DeleteUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return ResponseDto<NoContentDto>.Fail("Kullanıcı Bulunamadı.", 400);
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return ResponseDto<NoContentDto>.Fail(new ErrorDto(result.Errors.Select(p => p.Description).ToList()), 400);
            }

            return ResponseDto<NoContentDto>.Success(200);
        }

        public async Task<ResponseDto<NoContentDto>> GeneratePasswordResetTokenAndSendEmailAsync(PasswordResetTokenDto passwordResetTokenDto)
        {
            var userFromDb = await _userManager.FindByEmailAsync(passwordResetTokenDto.Email);

            if (userFromDb == null)
            {
                return ResponseDto<NoContentDto>.Fail("Bu e-posta'ya ait bir kullanıcı bulunamadı.", 404);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(userFromDb);

            // Send token to user
            var message = new MessageForEmail(new string[] { userFromDb.Email },
                "Parola Sıfırlama",
                $"<p>Aşağıdaki linke tıklayarak parolanızı sıfırlayabilirsiniz</p>" +
                    $"<p>{_appOptions.FrontEndBaseUrl + "Account/ResetPassword?email=" + userFromDb.Email + "&token=" + HttpUtility.UrlEncode(token)}</p>" +
                    $"<p></p>");
            _emailSender.SendEmail(message);

            return ResponseDto<NoContentDto>.Success(200);
        }

        public async Task<ResponseDto<NoContentDto>> ResetPasswordAsync(ResetPasswordDto user)
        {
            var userFromDb = await _userManager.FindByEmailAsync(user.Email.ToString());

            if (userFromDb == null)
            {
                return ResponseDto<NoContentDto>.Fail("Kullanıcı bulunamadı.", 404);
            }

            var result = await _userManager.ResetPasswordAsync(userFromDb, user.Token, user.Password);

            if (!result.Succeeded)
            {
                return ResponseDto<NoContentDto>.Fail(new ErrorDto(result.Errors.Select(x => x.Description).ToList()), 400);
            }

            return ResponseDto<NoContentDto>.Success(200);
        }

        public async Task<ResponseDto<NoContentDto>> GenerateChangeEmailTokenAndSendEmailAsync(Guid userId, string newEmail)
        {
            var userFromDb = await _userManager.FindByIdAsync(userId.ToString());

            if (userFromDb == null)
            {
                return ResponseDto<NoContentDto>.Fail("Kullanıcı bulunamadı.", 404);
            }

            string token = await _userManager.GenerateChangeEmailTokenAsync(userFromDb, newEmail);

            // Send token to user
            var message = new MessageForEmail(new string[] { newEmail },
                "E-posta Değişikliği",
                $"<p>Aşağıdaki linke tıklayarak yeni e-postanızı doğrulayabilirsiniz</p>" +
                    $"<p>{_appOptions.FrontEndBaseUrl + "Account/ChangeEmailConfirm?email=" + newEmail + 
                    "&token=" + HttpUtility.UrlEncode(token) + 
                    "&oldEmail=" + userFromDb.Email}</p>" +
                    $"<p></p>");
            _emailSender.SendEmail(message);

            return ResponseDto<NoContentDto>.Success(200);
        }

        public async Task<ResponseDto<NoContentDto>> ConfirmNewEmailAsync(string oldEmail, string newEmail, string token)
        {
            var user = await _userManager.FindByEmailAsync(oldEmail);

            if (user == null)
            {
                return ResponseDto<NoContentDto>.Fail("Bu Email'e ait kullanıcı bulunamadı.", 404);
            }

            var result = await _userManager.ChangeEmailAsync(user, newEmail, token);

            if (!result.Succeeded)
            {
                return ResponseDto<NoContentDto>.Fail(new ErrorDto(result.Errors.Select(x => x.Description).ToList()), 400);
            }

            return ResponseDto<NoContentDto>.Success(200);
        }

        public async Task<ResponseDto<NoContentDto>> ChangePasswordAsync(Guid userId, ChangePasswordDto password)
        {
            var userFromDb = await _userManager.FindByIdAsync(userId.ToString());

            if (userFromDb == null)
            {
                return ResponseDto<NoContentDto>.Fail("Kullanıcı bulunamadı.", 404);
            }

            var result = await _userManager.ChangePasswordAsync(userFromDb, password.CurrentPassword, password.NewPassword);

            if (!result.Succeeded)
            {
                return ResponseDto<NoContentDto>.Fail(new ErrorDto(result.Errors.Select(x => x.Description).ToList()), 400);
            }

            return ResponseDto<NoContentDto>.Success(200);
        }
    }
}
