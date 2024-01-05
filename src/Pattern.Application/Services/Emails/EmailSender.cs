using Microsoft.Extensions.Options;
using MimeKit;
using Pattern.Application.Services.Emails.Dtos;

namespace Pattern.Application.Services.Emails
{
	public class EmailSender : IEmailSender
	{
		private readonly EmailConfiguration _emailConfig;
		public EmailSender(IOptions<EmailConfiguration> emailConfig)
		{
			_emailConfig = emailConfig.Value;
		}
		public async Task SendEmailAsync(SendEmailDto sendEmailDto)
		{
			var emailMessage = CreateMailMessage(sendEmailDto);
			await SendAsync(emailMessage);
		}
		private MimeMessage CreateMailMessage(SendEmailDto sendEmailDto)
		{
			var emailMessage = new MimeMessage();
			emailMessage.From.Add(new MailboxAddress(_emailConfig.Name, _emailConfig.From));

			var To = new List<MailboxAddress>();
			To.AddRange(sendEmailDto.To.Select(p => new MailboxAddress("", p)));
			emailMessage.To.AddRange(To);
			emailMessage.Subject = sendEmailDto.Subject;

			var bodyBuilder = new BodyBuilder();
			if (sendEmailDto.BodyIsHtml)
			{
				bodyBuilder.HtmlBody = sendEmailDto.Body;
			}
			else
			{
				bodyBuilder.TextBody = sendEmailDto.Body;
			}

			if (sendEmailDto.Attachments != null && sendEmailDto.Attachments.Count > 0)
			{
				sendEmailDto.Attachments.ForEach(x =>
				{
					bodyBuilder.Attachments.Add(x.Name, x.Data, new ContentType(x.MediaType, x.MediaSubtype));
				});
			}

			return emailMessage;
		}
		private async Task SendAsync(MimeMessage emailMessage)
		{
			using (var client = new MailKit.Net.Smtp.SmtpClient())
			{
				try
				{
					await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, MailKit.Security.SecureSocketOptions.None);
					await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
					await client.SendAsync(emailMessage);
					await client.DisconnectAsync(true);
				}
				catch
				{
					throw;
				}
				finally
				{
					await client.DisconnectAsync(true);
					client.Dispose();
				}
			}
		}
	}
}
