using FluentValidation;
using Pattern.Application.Services.Users.Dtos;

namespace Pattern.Application.Services.Users.Validators
{
	public class CreateUserValidator : AbstractValidator<CreateUserDto>
	{
		public CreateUserValidator()
		{
			RuleFor(x => x.UserName)
				.NotNull().WithMessage("UserName is required.")
				.NotEmpty().WithMessage("UserName is required.");

			RuleFor(x => x.Email)
				.NotNull().WithMessage("Email is required.")
				.NotEmpty().WithMessage("Email is required.");

			RuleFor(x => x.Password)
				.NotNull().WithMessage("Password is required.")
				.NotEmpty().WithMessage("Password is required.");

			RuleFor(x => x.FirstName)
				.NotNull().WithMessage("FirstName is required.")
				.NotEmpty().WithMessage("FirstName is required.");

			RuleFor(x => x.LastName)
				.NotNull().WithMessage("LastName is required.")
				.NotEmpty().WithMessage("LastName is required.");

			RuleFor(x => x.PhoneNumber)
				.NotNull().WithMessage("PhoneNumber is required.")
				.NotEmpty().WithMessage("PhoneNumber is required.");
		}
	}

	public class UpdateProfileValidator : AbstractValidator<UpdateProfileDto>
	{
		public UpdateProfileValidator()
		{
			RuleFor(x => x.FirstName)
				.NotNull().WithMessage("FirstName is required.")
				.NotEmpty().WithMessage("FirstName is required.");

			RuleFor(x => x.LastName)
				.NotNull().WithMessage("LastName is required.")
				.NotEmpty().WithMessage("LastName is required.");

			RuleFor(x => x.PhoneNumber)
				.NotNull().WithMessage("PhoneNumber is required.")
				.NotEmpty().WithMessage("PhoneNumber is required.");
		}
	}

	public class PasswordResetValidator : AbstractValidator<PasswordResetTokenDto>
	{
		public PasswordResetValidator()
		{
			RuleFor(x => x.Email)
				.NotNull().WithMessage("Email is required.")
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("Email is not valid.");
		}
	}

	public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
	{
		public ResetPasswordValidator()
		{
			RuleFor(x => x.UserId)
				.NotNull().WithMessage("UserId is required.")
				.NotEmpty().WithMessage("UserId is required.");

			RuleFor(x => x.Token)
				.NotNull().WithMessage("Token is required.")
				.NotEmpty().WithMessage("Token is required.");

			RuleFor(x => x.NewPassword)
				.NotNull().WithMessage("NewPassword is required.")
				.NotEmpty().WithMessage("NewPassword is required.");
		}
	}

	public class EmailChangeValidator : AbstractValidator<SendEmailChangeEmailDto>
	{
		public EmailChangeValidator()
		{
			RuleFor(x => x.NewEmail)
				.NotNull().WithMessage("NewEmail is required.")
				.NotEmpty().WithMessage("NewEmail is required.")
				.EmailAddress().WithMessage("Email is not valid.");
		}
	}

	public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailDto>
	{
		public ConfirmEmailValidator()
		{
			RuleFor(x => x.UserId)
				.NotNull().WithMessage("UserId is required.")
				.NotEmpty().WithMessage("UserId is required.");

			RuleFor(x => x.Token)
				.NotNull().WithMessage("Token is required.")
				.NotEmpty().WithMessage("Token is required.");
		}
	}

	public class ConfirmNewEmailValidator : AbstractValidator<ConfirmNewEmailDto>
	{
		public ConfirmNewEmailValidator()
		{
			RuleFor(x => x.OldEmail)
				.NotNull().WithMessage("OldEmail is required.")
				.NotEmpty().WithMessage("OldEmail is required.");

			RuleFor(x => x.NewEmail)
				.NotNull().WithMessage("NewEmail is required.")
				.NotEmpty().WithMessage("NewEmail is required.");

			RuleFor(x => x.Token)
				.NotNull().WithMessage("Token is required.")
				.NotEmpty().WithMessage("Token is required.");
		}
	}

	public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
	{
		public ChangePasswordValidator()
		{
			RuleFor(x => x.CurrentPassword)
				.NotNull().WithMessage("Current password is required.")
				.NotEmpty().WithMessage("Current password is required.");

			RuleFor(x => x.NewPassword)
				.NotNull().WithMessage("New password is required.")
				.NotEmpty().WithMessage("New password is required.");
		}
	}
}
