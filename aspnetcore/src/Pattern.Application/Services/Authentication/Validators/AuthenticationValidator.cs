using FluentValidation;
using Pattern.Application.Services.Authentication.Dtos;

namespace Pattern.Application.Services.Authentication.Validators
{
	public class LoginValidator : AbstractValidator<LoginDto>
	{
		public LoginValidator()
		{
			RuleFor(x => x.Email)
				.NotNull().WithMessage("Email is required.")
				.NotEmpty().WithMessage("Email is required.");

			RuleFor(x => x.Password)
				.NotNull().WithMessage("Password is required.")
				.NotEmpty().WithMessage("Password is required.");
		}
	}

	public class RefreshTokenValidator : AbstractValidator<RefreshTokenDto>
	{
		public RefreshTokenValidator()
		{
			RuleFor(x => x.Token)
				.NotNull().WithMessage("RefreshToken is required.")
				.NotEmpty().WithMessage("RefreshToken is required.");
		}
	}
}
