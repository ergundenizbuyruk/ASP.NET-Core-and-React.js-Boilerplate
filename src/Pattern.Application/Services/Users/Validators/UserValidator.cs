using FluentValidation;
using Pattern.Application.Services.Users.Dtos;

namespace Pattern.Application.Services.Users.Validators
{
    public class UserValidator : AbstractValidator<ChangePasswordDto>
    {
        public UserValidator()
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
