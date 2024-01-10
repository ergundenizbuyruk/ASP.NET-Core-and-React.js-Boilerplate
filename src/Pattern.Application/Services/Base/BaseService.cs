using AutoMapper;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Base
{
	public abstract class BaseService : IBaseService
	{
		private readonly IUnitOfWork unitOfWork;
		protected IMapper ObjectMapper { get; private set; }

		public BaseService(IUnitOfWork unitOfWork, IMapper objectMapper)
		{
			this.unitOfWork = unitOfWork;
			ObjectMapper = objectMapper;
		}

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
