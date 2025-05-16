using AutoMapper;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Base
{
    public abstract class BaseService(IUnitOfWork unitOfWork, IMapper objectMapper) : IBaseService
    {
        protected IMapper ObjectMapper { get; private set; } = objectMapper;

        public async Task SaveChangesAsync()
        {
            await unitOfWork.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            unitOfWork.SaveChanges();
        }
    }
}