﻿using Pattern.Application.Services.Base.Dtos;

namespace Pattern.Application.Services.Users.Dtos
{
    public class UpdateUserDto : EntityDto<Guid>
    {
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
	}
}
