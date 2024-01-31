using AutoMapper;
using Pattern.Application.Services.Roles.Dtos;
using Pattern.Core.Entites.Authentication;

namespace Pattern.Application.Services.Roles.Mapper
{
	public class RoleMapper : Profile
	{
		public RoleMapper()
		{
			CreateMap<Permission, PermissionDto>();
			CreateMap<Role, RoleDto>();
		}
	}
}
