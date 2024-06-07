using Pattern.Application.Services.Base.Dtos;
using Pattern.Core.Entites.BaseEntity;
using Pattern.Core.Responses;

namespace Pattern.Application.Services.Base
{
    public interface ICrudService<TEntity, TPrimaryKey, TEntityDto, TCreateDto, TUpdateDto> : IApplicationService
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateDto : IEntityDto<TPrimaryKey>
    {
        Task<ResponseDto<TEntityDto>> CreateAsync(TCreateDto createDto);
        Task<ResponseDto<TEntityDto>> UpdateAsync(TUpdateDto updateDto);
        Task<ResponseDto<NoContentDto>> DeleteAsync(TPrimaryKey primaryKey);
        Task<ResponseDto<TEntityDto>> GetAsync(TPrimaryKey primaryKey);
        Task<ResponseDto<List<TEntityDto>>> GetAllAsync(int? pageNumber = null, int? pageSize = null);
    }
}
