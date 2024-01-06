using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pattern.Core.Entites.BaseEntity;
using Pattern.Core.Exceptions;
using Pattern.Persistence.Repositories;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Base
{
	public class CrudService<TEntity, TPrimaryKey, TDto, TCreateDto, TUpdateDto> : ApplicationService, ICrudService where TEntity : Entity<TPrimaryKey>
	{
		private readonly IRepository<TEntity, TPrimaryKey> repository;
		public CrudService(IUnitOfWork unitOfWork, IMapper objectMapper, IRepository<TEntity, TPrimaryKey> repository) : base(unitOfWork, objectMapper)
		{
			this.repository = repository;
		}

		protected async Task<TDto> CreateAsync(TCreateDto createDto)
		{
			var entity = ObjectMapper.Map<TCreateDto, TEntity>(createDto);
			await repository.CreateAsync(entity);
			await SaveChangesAsync();
			return ObjectMapper.Map<TEntity, TDto>(entity);
		}

		protected async Task<TDto> UpdateAsync(TUpdateDto updateAsync)
		{
			var entity = ObjectMapper.Map<TUpdateDto, TEntity>(updateAsync);
			var entityFromDb = await repository.GetAsync(entity.Id);

			if (entityFromDb == null)
			{
				throw new EntityNotFoundException(nameof(TEntity));
			}

			repository.SetValuesAndUpdate(entityFromDb, entity);
			await SaveChangesAsync();
			return ObjectMapper.Map<TEntity, TDto>(entity);
		}

		protected async Task DeleteAsync(TPrimaryKey primaryKey)
		{
			var entityFromDb = await repository.GetAsync(primaryKey);

			if (entityFromDb == null)
			{
				throw new EntityNotFoundException(nameof(TEntity));
			}

			repository.Delete(entityFromDb);
		}

		protected async Task<TDto> GetAsync(TPrimaryKey primaryKey)
		{
			var entityFromDb = await repository.GetAsync(primaryKey);

			if (entityFromDb == null)
			{
				throw new EntityNotFoundException(nameof(TEntity));
			}

			return ObjectMapper.Map<TEntity, TDto>(entityFromDb);
		}

		// TO DO: i will add paging
		protected async Task<List<TDto>> GetAllAsync()
		{
			var entities = await repository.GetAllAsync();

			if (entities == null)
			{
				throw new EntityNotFoundException(nameof(TEntity));
			}

			return ObjectMapper.Map<List<TEntity>, List<TDto>>(entities);
		}
	}
}
