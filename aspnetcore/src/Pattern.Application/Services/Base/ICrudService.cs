using Pattern.Application.Services.Base.Dtos;
using Pattern.Core.Entites.BaseEntity;

namespace Pattern.Application.Services.Base
{
    public interface ICrudService<TEntity, TPrimaryKey, TEntityDto, TCreateDto, TUpdateDto> : IApplicationService
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : class, IEntityDto<TPrimaryKey>
        where TUpdateDto : class, IEntityDto<TPrimaryKey>
    {
        Task<TEntityDto> CreateAsync(TCreateDto createDto);
        Task<TEntityDto> UpdateAsync(TUpdateDto updateDto);
        Task DeleteAsync(TPrimaryKey primaryKey);
        Task<TEntityDto> GetAsync(TPrimaryKey primaryKey);
        Task<List<TEntityDto>> GetAllAsync();
    }
}