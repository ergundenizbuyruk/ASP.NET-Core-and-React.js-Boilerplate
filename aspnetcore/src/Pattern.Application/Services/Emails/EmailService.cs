﻿using AutoMapper;
using Microsoft.Extensions.Options;
using Pattern.Application.Services.Base;
using Pattern.Application.Services.Emails.Dtos;
using Pattern.Core.Options;
using Pattern.Persistence.UnitOfWork;
using System.Web;

namespace Pattern.Application.Services.Emails
{
	public class EmailService : ApplicationService, IEmailService
	{
		private readonly IEmailSender emailSender;
		private readonly FrontInformation frontInformation;

		public EmailService(IUnitOfWork unitOfWork, IMapper objectMapper, IEmailSender emailSender, IOptions<FrontInformation> frontInformation) : base(unitOfWork, objectMapper)
		{
			this.emailSender = emailSender;
			this.frontInformation = frontInformation.Value;
		}

		public async Task SendEmailConfirmEmailAsync(string userEmail, Guid userId, string token)
		{
			var emailDto = new SendEmailDto()
			{
				To = new List<string>() { userEmail },
				BodyIsHtml = true,
				Subject = "E-posta Doğrulama",
				Body = $"<a href=\"{frontInformation.EmailConfirmUrl + "?id=" + userId.ToString() + "&token=" +
				token}\" target=\"_blank\">Buraya</a>" +
				$" tıklayarak e-postanızı doğrulayabilirsiniz." +
				"<p>Tıklayamıyorsanız aşağıdaki linki kopyalayarak ilerleyebilirsiniz.</p>" +
					$"<p>{frontInformation.EmailConfirmUrl + "?id=" + userId.ToString() + "&token=" + token}</p>" +
					$"<p></p>"
			};
			await emailSender.SendEmailAsync(emailDto);
		}

		public async Task SendChangeEmailAsync(string oldEmail, string newEmail, string token)
		{
			var emailDto = new SendEmailDto()
			{
				To = new List<string>() { newEmail },
				BodyIsHtml = true,
				Subject = "E-posta Değişikliği",
				Body = $"<a href=\"{frontInformation.ChangeEmailConfirmUrl + "?newEmail=" + newEmail + "&token=" + token + "&oldEmail=" + oldEmail}\" target=\"_blank\">Buraya</a>" +
				" tıklayarak yeni e-postanızı doğrulayabilirsiniz." +
				"<p>Tıklayamıyorsanız aşağıdaki linki kopyalayarak ilerleyebilirsiniz.</p>" +
					$"<p>{frontInformation.ChangeEmailConfirmUrl + "?newEmail=" + newEmail + "&token=" + token + "&oldEmail=" + oldEmail}</p>" +
					$"<p></p>"
			};
			await emailSender.SendEmailAsync(emailDto);
		}

		public async Task SendPasswordResetEmailAsync(string userEmail, Guid userId, string token)
		{
			var emailDto = new SendEmailDto()
			{
				To = new List<string>() { userEmail },
				BodyIsHtml = true,
				Subject = "Parola Sıfırlama",
				Body = $"<a href=\"{frontInformation.PasswordResetUrl + "?id=" + userId + "&token=" + token}\" target=\"_blank\">Buraya</a>" +
				" tıklayarak parolanızı sıfırlayabilirsiniz." +
				"<p>Tıklayamıyorsanız aşağıdaki linki kopyalayarak ilerleyebilirsiniz.</p>" +
					$"<p>{frontInformation.PasswordResetUrl + "?id=" + userId + "&token=" + token}</p>" +
					$"<p></p>"
			};
			await emailSender.SendEmailAsync(emailDto);
		}
	}
}
