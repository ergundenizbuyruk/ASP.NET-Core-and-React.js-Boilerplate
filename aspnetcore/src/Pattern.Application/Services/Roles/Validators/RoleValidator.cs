using FluentValidation;
using Pattern.Application.Services.Roles.Dtos;

namespace Pattern.Application.Services.Roles.Validators
{
	public class CreateRoleValidator : AbstractValidator<CreateRoleDto>
	{
		public CreateRoleValidator()
		{
			RuleFor(x => x.Name)
				.NotNull().WithMessage("Name is required.")
				.NotEmpty().WithMessage("Name is required.");

			RuleFor(x => x.PermissionIds)
				.NotNull().WithMessage("PermissionIds is required.");
		}
	}

	public class UpdateRoleValidator : AbstractValidator<UpdateRoleDto>
	{
		public UpdateRoleValidator()
		{
			RuleFor(x => x.Id)
				.NotNull().WithMessage("Id is required.")
				.NotEmpty().WithMessage("Id is required.");

			RuleFor(x => x.PermissionIds)
				.NotNull().WithMessage("PermissionIds is required.");

			RuleFor(x => x.Name)
				.NotNull().WithMessage("Name is required.")
				.NotEmpty().WithMessage("Name is required.");
		}
	}
}
