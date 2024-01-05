using AutoMapper;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services
{
	public class BaseService
	{
		private readonly IUnitOfWork _unitOfWork;
		protected IMapper ObjectMapper { get; private set; }

		public BaseService(IUnitOfWork unitOfWork, IMapper objectMapper)
		{
			_unitOfWork = unitOfWork;
			ObjectMapper = objectMapper;
		}

		protected void SaveChanges()
		{
			_unitOfWork.SaveChanges();
		}

		protected async Task SaveChangesAsync()
		{

			await _unitOfWork.SaveChangesAsync();
		}
	}
}
