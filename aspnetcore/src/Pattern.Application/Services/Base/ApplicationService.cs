using AutoMapper;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Base
{
    public abstract class ApplicationService(IUnitOfWork unitOfWork, IMapper objectMapper)
        : BaseService(unitOfWork, objectMapper), IApplicationService;
}