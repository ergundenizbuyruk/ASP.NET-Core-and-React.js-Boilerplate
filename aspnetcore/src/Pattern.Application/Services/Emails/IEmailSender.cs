using Pattern.Application.Services.Emails.Dtos;

namespace Pattern.Application.Services.Emails
{
	public interface IEmailSender
	{
		public Task SendEmailAsync(SendEmailDto sendEmailDto);
	}
}
