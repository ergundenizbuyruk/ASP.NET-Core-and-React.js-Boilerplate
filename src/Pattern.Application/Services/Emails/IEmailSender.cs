namespace Pattern.Application.Services.Emails
{
    public interface IEmailSender
    {
        void SendEmail(MessageForEmail message);
    }
}
