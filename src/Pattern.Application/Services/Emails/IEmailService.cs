namespace Pattern.Application.Services.Emails
{
	public interface IEmailService
	{
		public Task SendEmailConfirmEmailAsync(string userEmail, Guid userId, string token);
		public Task SendChangeEmailAsync(string oldEmail, string newEmail, string token);
		public Task SendPasswordResetEmailAsync(string userEmail, Guid userId, string token);
	}
}
