using Microsoft.AspNetCore.Identity;

namespace Pattern.Core.Localization
{
	public class LocalizationIdentityErrorDescriber : IdentityErrorDescriber
	{

		public override IdentityError DuplicateEmail(string email)
		{
			return new IdentityError
			{
				Code = nameof(DuplicateEmail),
				Description = $"'{email}' daha önce başka bir kullanıcı taradınfan alınmıştır."
			};
		}

		public override IdentityError DuplicateUserName(string userName)
		{
			return new IdentityError
			{
				Code = nameof(DuplicateUserName),
				Description = $"'{userName}' daha önce başka bir kullanıcı tarafından alınmıştır."
			};
		}

		public override IdentityError PasswordRequiresDigit()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresDigit),
				Description = $"Parolanız en az 1 rakam içermelidir."
			};
		}

		public override IdentityError PasswordTooShort(int length)
		{
			return new IdentityError
			{
				Code = nameof(PasswordTooShort),
				Description = $"Parolanız en az 6 karakterden oluşmalıdır."
			};
		}

		public override IdentityError PasswordRequiresLower()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresLower),
				Description = $"Parolanız en az 1 küçük harf içermelidir."
			};
		}

		public override IdentityError PasswordRequiresNonAlphanumeric()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresNonAlphanumeric),
				Description = $"Parolanız en az 1 harf ve sayı olmayan karakter içermelidir."
			};
		}

		public override IdentityError PasswordRequiresUpper()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresUpper),
				Description = $"Parolanız en az 1 büyük harf içermelidir."
			};
		}
	}
}
