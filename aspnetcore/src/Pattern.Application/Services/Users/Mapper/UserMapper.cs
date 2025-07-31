using AutoMapper;
using Pattern.Application.Services.Users.Dtos;
using Pattern.Core.Entites.Authentication;

namespace Pattern.Application.Services.Users.Mapper
{
	public class UserMapper : Profile
	{
		public UserMapper()
		{
			CreateMap<User, UserDto>();
			CreateMap<CreateUserDto, User>();
			CreateMap<UpdateUserDto, User>();
		}
	}
}
