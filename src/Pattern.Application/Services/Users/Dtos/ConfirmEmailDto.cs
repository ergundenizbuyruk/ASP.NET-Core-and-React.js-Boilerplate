namespace Pattern.Application.Services.Users.Dtos
{
	public class ConfirmEmailDto
	{
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}
