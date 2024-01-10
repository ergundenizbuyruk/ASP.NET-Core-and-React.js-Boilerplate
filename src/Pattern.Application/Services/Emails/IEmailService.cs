using Pattern.Application.Services.Base;

namespace Pattern.Application.Services.Emails
{
	public interface IEmailService : IApplicationService
	{
		public Task SendEmailConfirmEmailAsync(string userEmail, Guid userId, string token);
		public Task SendChangeEmailAsync(string oldEmail, string newEmail, string token);
		public Task SendPasswordResetEmailAsync(string userEmail, Guid userId, string token);
	}
}
