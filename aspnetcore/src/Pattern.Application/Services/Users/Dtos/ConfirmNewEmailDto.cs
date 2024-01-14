namespace Pattern.Application.Services.Users.Dtos
{
    public class ConfirmNewEmailDto
    {
        public string OldEmail { get; set; }
        public string Token { get; set; }
        public string NewEmail { get; set; }
    }
}
