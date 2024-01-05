using AutoMapper;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Roles
{
	public class RoleService : BaseService
    {
        public RoleService(IUnitOfWork unitOfWork, IMapper objectMapper) : base(unitOfWork, objectMapper)
        {
        }
    }
}
