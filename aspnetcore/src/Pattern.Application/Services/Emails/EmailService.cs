using AutoMapper;
using Microsoft.Extensions.Options;
using Pattern.Application.Services.Base;
using Pattern.Application.Services.Emails.Dtos;
using Pattern.Core.Options;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Emails;

public class EmailService(
    IUnitOfWork unitOfWork,
    IMapper objectMapper,
    IEmailSender emailSender,
    IOptions<FrontInformation> frontInformation)
    : ApplicationService(unitOfWork, objectMapper), IEmailService
{
    private readonly FrontInformation frontInformation = frontInformation.Value;

    public async Task SendEmailConfirmEmailAsync(string userEmail, Guid userId, string token)
    {
        var emailDto = new SendEmailDto()
        {
            To = [userEmail],
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
            To = [newEmail],
            BodyIsHtml = true,
            Subject = "E-posta Değişikliği",
            Body =
                $"<a href=\"{frontInformation.ChangeEmailConfirmUrl + "?newEmail=" + newEmail + "&token=" + token + "&oldEmail=" + oldEmail}\" target=\"_blank\">Buraya</a>" +
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
            To = [userEmail],
            BodyIsHtml = true,
            Subject = "Parola Sıfırlama",
            Body =
                $"<a href=\"{frontInformation.PasswordResetUrl + "?id=" + userId + "&token=" + token}\" target=\"_blank\">Buraya</a>" +
                " tıklayarak parolanızı sıfırlayabilirsiniz." +
                "<p>Tıklayamıyorsanız aşağıdaki linki kopyalayarak ilerleyebilirsiniz.</p>" +
                $"<p>{frontInformation.PasswordResetUrl + "?id=" + userId + "&token=" + token}</p>" +
                $"<p></p>"
        };
        await emailSender.SendEmailAsync(emailDto);
    }
}