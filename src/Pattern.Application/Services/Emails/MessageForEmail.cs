using MimeKit;

namespace Pattern.Application.Services.Emails
{
    public class MessageForEmail
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string VerificationCode { get; set; }
        public MessageForEmail(IEnumerable<string> to, string subject, string verificationCode)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress("", x)));
            Subject = subject;
            VerificationCode = verificationCode;
        }
    }
}
