using Pattern.Core.Entites.BaseEntity;

namespace Pattern.Application.Services.Users.Dtos
{
    public class UpdateUserDto : EntityDto<Guid>
    {
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime? BirthDate { get; set; }
		public string PhoneNumber { get; set; }
	}
}
