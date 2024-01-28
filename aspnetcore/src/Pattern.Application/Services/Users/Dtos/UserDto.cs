using Pattern.Application.Services.Base.Dtos;

namespace Pattern.Application.Services.Users.Dtos
{
    public class UserDto : EntityDto<Guid>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public bool IsActive { get; set; }
	}
}
