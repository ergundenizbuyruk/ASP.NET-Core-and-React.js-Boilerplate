namespace Pattern.Application.Services.Users.Dtos
{
    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
	}
}
