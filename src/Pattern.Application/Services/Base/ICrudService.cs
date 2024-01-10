using Pattern.Application.Services.Base.Dtos;
using Pattern.Core.Entites.BaseEntity;

namespace Pattern.Application.Services.Base
{
	public interface ICrudService<TEntity, TPrimaryKey, TEntityDto, TCreateDto, TUpdateDto> : IApplicationService
		where TEntity : class, IEntity<TPrimaryKey>
		where TEntityDto : IEntityDto<TPrimaryKey>
		where TUpdateDto : IEntityDto<TPrimaryKey>
	{
		Task<TEntityDto> CreateAsync(TCreateDto createDto);
		Task<TEntityDto> UpdateAsync(TUpdateDto updateAsync);
		Task DeleteAsync(TPrimaryKey primaryKey);
		Task<TEntityDto> GetAsync(TPrimaryKey primaryKey);
		Task<List<TEntityDto>> GetAllAsync(GetAllDto? getAllDto);
	}
}
