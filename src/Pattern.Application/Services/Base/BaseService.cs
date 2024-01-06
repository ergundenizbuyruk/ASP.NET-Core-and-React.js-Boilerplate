using AutoMapper;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Base
{
    public abstract class BaseService : IBaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        protected IMapper ObjectMapper { get; private set; }

		public BaseService(IUnitOfWork unitOfWork, IMapper objectMapper)
        {
            _unitOfWork = unitOfWork;
            ObjectMapper = objectMapper;
        }

		public async Task SaveChangesAsync()
		{
			await _unitOfWork.SaveChangesAsync();
		}

		public void SaveChanges()
		{
			_unitOfWork.SaveChanges();
		}
	}
}
