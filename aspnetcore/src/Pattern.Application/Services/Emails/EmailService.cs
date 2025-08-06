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

    public async Task SendEmailConfirmationTokenAsync(string userEmail, string token)
    {
        var emailDto = new SendEmailDto
        {
            To = [userEmail],
            BodyIsHtml = true,
            Subject = "E-posta Doğrulama",
            Body = $"E-posta doğrulama kodunuz: {token}"
        };

        await emailSender.SendEmailAsync(emailDto);
    }

    public async Task SendChangeEmailAsync(string newEmail, string token)
    {
        var emailDto = new SendEmailDto
        {
            To = [newEmail],
            BodyIsHtml = true,
            Subject = "E-posta Değişikliği",
            Body = "E-posta güncelleme işlemi için kodunuz: " + token
        };
        await emailSender.SendEmailAsync(emailDto);
    }

    public async Task SendPasswordResetEmailAsync(string userEmail, string token)
    {
        var emailDto = new SendEmailDto
        {
            To = [userEmail],
            BodyIsHtml = true,
            Subject = "Parola Sıfırlama",
            Body = $"Parola sıfırlama işlemi için kodunuz: {token}"
        };
        await emailSender.SendEmailAsync(emailDto);
    }
}