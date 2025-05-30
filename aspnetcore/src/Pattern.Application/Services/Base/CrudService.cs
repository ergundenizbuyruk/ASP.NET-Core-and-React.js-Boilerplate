using AutoMapper;
using Pattern.Application.Services.Base.Dtos;
using Pattern.Core;
using Pattern.Core.Entites.BaseEntity;
using Pattern.Core.Exceptions;
using Pattern.Persistence.Repositories;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.Application.Services.Base;

public abstract class CrudService<TEntity, TPrimaryKey, TEntityDto, TCreateDto, TUpdateDto>(
    IUnitOfWork unitOfWork,
    IMapper objectMapper,
    IRepository<TEntity, TPrimaryKey> repository,
    IResourceLocalizer localizer) :
    ApplicationService(unitOfWork, objectMapper),
    ICrudService<TEntity, TPrimaryKey, TEntityDto, TCreateDto, TUpdateDto>
    where TEntity : class, IEntity<TPrimaryKey>
    where TEntityDto : IEntityDto<TPrimaryKey>
    where TUpdateDto : IEntityDto<TPrimaryKey>
{
    protected readonly IRepository<TEntity, TPrimaryKey> Repository = repository;

    public virtual async Task<TEntityDto> CreateAsync(TCreateDto createDto)
    {
        var entity = ObjectMapper.Map<TCreateDto, TEntity>(createDto);
        await Repository.CreateAsync(entity);
        await SaveChangesAsync();
        return ObjectMapper.Map<TEntity, TEntityDto>(entity);
    }

    public virtual async Task<TEntityDto> UpdateAsync(TUpdateDto updateDto)
    {
        var entityFromDb = await Repository.GetByIdAsync(updateDto.Id);

        if (entityFromDb is null)
        {
            throw new NotFoundException($"{nameof(TEntity)} {localizer.Localize("NotFound")}");
        }

        ObjectMapper.Map(updateDto, entityFromDb);
        Repository.Update(entityFromDb);
        await SaveChangesAsync();
        return ObjectMapper.Map<TEntity, TEntityDto>(entityFromDb);
    }

    public virtual async Task DeleteAsync(TPrimaryKey primaryKey)
    {
        var entityFromDb = await Repository.GetByIdAsync(primaryKey);

        if (entityFromDb is null)
        {
            throw new NotFoundException($"{nameof(TEntity)} {localizer.Localize("NotFound")}");
        }

        Repository.Delete(entityFromDb);
        await SaveChangesAsync();
    }

    public virtual async Task<TEntityDto> GetAsync(TPrimaryKey primaryKey)
    {
        var entityFromDb = await Repository.GetByIdAsync(primaryKey);

        if (entityFromDb is null)
        {
            throw new NotFoundException($"{nameof(TEntity)} {localizer.Localize("NotFound")}");
        }

        return ObjectMapper.Map<TEntity, TEntityDto>(entityFromDb);
    }

    public virtual async Task<List<TEntityDto>> GetAllAsync(int? pageNumber = null,
        int? pageSize = null)
    {
        var entities = await Repository.GetAllAsync(pageNumber, pageSize);
        return ObjectMapper.Map<List<TEntity>, List<TEntityDto>>(entities);
    }
}