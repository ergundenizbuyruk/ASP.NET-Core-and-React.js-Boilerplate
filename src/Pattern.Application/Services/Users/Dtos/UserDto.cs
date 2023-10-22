using Pattern.Core.Entites.BaseEntity;

namespace Pattern.Application.Services.Users.Dtos
{
    public class UserDto : EntityDto<Guid>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime? BirthDate { get; set; }
		public string PhoneNumber { get; set; }
		public bool IsActive { get; set; }
	}
}
