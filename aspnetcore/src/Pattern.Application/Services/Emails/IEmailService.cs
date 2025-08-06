using Pattern.Application.Services.Base;

namespace Pattern.Application.Services.Emails
{
	public interface IEmailService : IApplicationService
	{
		public Task SendEmailConfirmationTokenAsync(string userEmail, string token);
		public Task SendChangeEmailAsync(string newEmail, string token);
		public Task SendPasswordResetEmailAsync(string userEmail, string token);
	}
}
