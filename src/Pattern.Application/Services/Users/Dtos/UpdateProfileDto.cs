namespace Pattern.Application.Services.Users.Dtos
{
	public class UpdateProfileDto
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime? BirthDate { get; set; }
		public string PhoneNumber { get; set; }
	}
}
