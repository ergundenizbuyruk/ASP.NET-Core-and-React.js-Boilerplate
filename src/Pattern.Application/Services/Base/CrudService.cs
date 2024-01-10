using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pattern.Application.Services.Base.Dtos;
using Pattern.Core.Entites.BaseEntity;
using Pattern.Core.Exceptions;
using Pattern.Persistence.Repositories;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Base
{
	public abstract class CrudService<TEntity, TPrimaryKey, TEntityDto, TCreateDto, TUpdateDto> :
		ApplicationService, ICrudService<TEntity, TPrimaryKey, TEntityDto, TCreateDto, TUpdateDto>
		where TEntity : class, IEntity<TPrimaryKey>
		where TEntityDto : IEntityDto<TPrimaryKey>
		where TUpdateDto : IEntityDto<TPrimaryKey>
	{
		private readonly IRepository<TEntity, TPrimaryKey> repository;
		protected CrudService(IUnitOfWork unitOfWork, IMapper objectMapper, IRepository<TEntity, TPrimaryKey> repository) : base(unitOfWork, objectMapper)
		{
			this.repository = repository;
		}

		public async Task<TEntityDto> CreateAsync(TCreateDto createDto)
		{
			var entity = ObjectMapper.Map<TCreateDto, TEntity>(createDto);
			await repository.CreateAsync(entity);
			await SaveChangesAsync();
			return ObjectMapper.Map<TEntity, TEntityDto>(entity);
		}

		public async Task<TEntityDto> UpdateAsync(TUpdateDto updateAsync)
		{
			var entity = ObjectMapper.Map<TUpdateDto, TEntity>(updateAsync);
			var entityFromDb = await repository.GetAsync(entity.Id);

			if (entityFromDb == null)
			{
				throw new EntityNotFoundException(nameof(TEntity));
			}

			repository.SetValuesAndUpdate(entityFromDb, entity);
			await SaveChangesAsync();
			return ObjectMapper.Map<TEntity, TEntityDto>(entity);
		}

		public async Task DeleteAsync(TPrimaryKey primaryKey)
		{
			var entityFromDb = await repository.GetAsync(primaryKey);

			if (entityFromDb == null)
			{
				throw new EntityNotFoundException(nameof(TEntity));
			}

			repository.Delete(entityFromDb);
		}

		public async Task<TEntityDto> GetAsync(TPrimaryKey primaryKey)
		{
			var entityFromDb = await repository.GetAsync(primaryKey);

			if (entityFromDb == null)
			{
				throw new EntityNotFoundException(nameof(TEntity));
			}

			return ObjectMapper.Map<TEntity, TEntityDto>(entityFromDb);
		}

		public async Task<List<TEntityDto>> GetAllAsync(GetAllDto? getAllDto)
		{
			var entities = await repository.GetAllAsync(getAllDto?.PageCount, getAllDto?.PageSize);

			if (entities == null)
			{
				throw new EntityNotFoundException(nameof(TEntity));
			}

			return ObjectMapper.Map<List<TEntity>, List<TEntityDto>>(entities);
		}
	}
}
