namespace Pattern.Application.Services.Emails
{ 
    public class EmailConfiguration
    {
        public string Name { get; set; }
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
