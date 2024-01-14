using AutoMapper;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Base
{
	public abstract class ApplicationService : BaseService, IApplicationService
	{
		protected ApplicationService(IUnitOfWork unitOfWork, IMapper objectMapper) : base(unitOfWork, objectMapper)
		{
		}
	}
}

