using AutoMapper;
using Pattern.Application.Services.Base.Dtos;
using Pattern.Core.Entites.BaseEntity;
using Pattern.Core.Exceptions;
using Pattern.Core.Responses;
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

		public virtual async Task<ResponseDto<TEntityDto>> CreateAsync(TCreateDto createDto)
		{
			var entity = ObjectMapper.Map<TCreateDto, TEntity>(createDto);
			await repository.CreateAsync(entity);
			await SaveChangesAsync();
			TEntityDto entityDto = ObjectMapper.Map<TEntity, TEntityDto>(entity);
			return ResponseDto<TEntityDto>.Success(entityDto, 201);
		}

		public virtual async Task<ResponseDto<TEntityDto>> UpdateAsync(TUpdateDto updateAsync)
		{
			var entity = ObjectMapper.Map<TUpdateDto, TEntity>(updateAsync);
			var entityFromDb = await repository.GetByIdAsync(entity.Id);

			if (entityFromDb == null)
			{
				throw new EntityNotFoundException(nameof(TEntity));
			}

			repository.SetValuesAndUpdate(entityFromDb, entity);
			await SaveChangesAsync();
			TEntityDto entityDto = ObjectMapper.Map<TEntity, TEntityDto>(entity);
			return ResponseDto<TEntityDto>.Success(entityDto, 200);
		}

		public virtual async Task<ResponseDto<NoContentDto>> DeleteAsync(TPrimaryKey primaryKey)
		{
			var entityFromDb = await repository.GetByIdAsync(primaryKey);

			if (entityFromDb == null)
			{
				throw new EntityNotFoundException(nameof(TEntity));
			}

			repository.Delete(entityFromDb);
			await SaveChangesAsync();
			return ResponseDto<NoContentDto>.Success(200);
		}

		public virtual async Task<ResponseDto<TEntityDto>> GetAsync(TPrimaryKey primaryKey)
		{
			var entityFromDb = await repository.GetByIdAsync(primaryKey);

			if (entityFromDb == null)
			{
				throw new EntityNotFoundException(nameof(TEntity));
			}

			TEntityDto entityDto = ObjectMapper.Map<TEntity, TEntityDto>(entityFromDb);
			return ResponseDto<TEntityDto>.Success(entityDto, 200);
		}

		public virtual async Task<ResponseDto<List<TEntityDto>>> GetAllAsync(int? pageNumber, int? pageSize)
		{
			var entities = await repository.GetAllAsync(pageNumber, pageSize);

			if (entities == null)
			{
				throw new EntityNotFoundException(nameof(TEntity));
			}

			var entityDtos = ObjectMapper.Map<List<TEntity>, List<TEntityDto>>(entities);
			return ResponseDto<List<TEntityDto>>.Success(entityDtos, 200);
		}
	}
}
