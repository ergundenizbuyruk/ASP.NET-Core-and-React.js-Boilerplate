using AutoMapper;
using Pattern.Application.Services.Base.Dtos;
using Pattern.Core.Entites.BaseEntity;
using Pattern.Core.Exceptions;
using Pattern.Core.Responses;
using Pattern.Persistence.Repositories;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Base
{
    public abstract class CrudService<TEntity, TPrimaryKey, TEntityDto, TCreateDto, TUpdateDto>(
        IUnitOfWork unitOfWork,
        IMapper objectMapper,
        IRepository<TEntity, TPrimaryKey> repository)
        :
            ApplicationService(unitOfWork, objectMapper),
            ICrudService<TEntity, TPrimaryKey, TEntityDto, TCreateDto, TUpdateDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateDto : IEntityDto<TPrimaryKey>
    {
        protected readonly IRepository<TEntity, TPrimaryKey> repository = repository;

        public virtual async Task<TEntityDto> CreateAsync(TCreateDto createDto)
        {
            var entity = ObjectMapper.Map<TCreateDto, TEntity>(createDto);
            await repository.CreateAsync(entity);
            await SaveChangesAsync();
            TEntityDto entityDto = ObjectMapper.Map<TEntity, TEntityDto>(entity);
            return entityDto;
        }

        public virtual async Task<TEntityDto> UpdateAsync(TUpdateDto updateDto)
        {
            var entityFromDb = await repository.GetByIdAsync(updateDto.Id);

            if (entityFromDb == null)
            {
                throw new NotFoundException(nameof(TEntity) + "is not found");
            }

            ObjectMapper.Map(updateDto, entityFromDb);
            repository.Update(entityFromDb);
            await SaveChangesAsync();
            return ObjectMapper.Map<TEntity, TEntityDto>(entityFromDb);
        }

        public virtual async Task DeleteAsync(TPrimaryKey primaryKey)
        {
            var entityFromDb = await repository.GetByIdAsync(primaryKey);

            if (entityFromDb == null)
            {
                throw new NotFoundException(nameof(TEntity) + "is not found");
            }

            repository.Delete(entityFromDb);
            await SaveChangesAsync();
        }

        public virtual async Task<TEntityDto> GetAsync(TPrimaryKey primaryKey)
        {
            var entityFromDb = await repository.GetByIdAsync(primaryKey);

            if (entityFromDb == null)
            {
                throw new NotFoundException(nameof(TEntity) + "is not found");
            }

            return ObjectMapper.Map<TEntity, TEntityDto>(entityFromDb);
        }

        public virtual async Task<List<TEntityDto>> GetAllAsync(int? pageNumber = null,
            int? pageSize = null)
        {
            var entities = await repository.GetAllAsync(pageNumber, pageSize);

            if (entities == null)
            {
                throw new NotFoundException(nameof(TEntity) + "is not found");
            }

            return ObjectMapper.Map<List<TEntity>, List<TEntityDto>>(entities);
        }
    }
}