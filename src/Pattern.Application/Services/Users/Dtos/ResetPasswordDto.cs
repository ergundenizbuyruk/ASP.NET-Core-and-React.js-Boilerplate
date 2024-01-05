namespace Pattern.Application.Services.Users.Dtos
{
	public class ResetPasswordDto
	{
		public Guid UserId { get; set; }
		public string Token { get; set; }
		public string NewPassword { get; set; }
	}
}
