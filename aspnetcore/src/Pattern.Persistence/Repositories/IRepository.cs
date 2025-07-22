using Pattern.Core.Entites.BaseEntity;

namespace Pattern.Persistence.Repositories;

public interface IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
{
    IQueryable<TEntity> GetAll();
    Task<List<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(TPrimaryKey key);
    Task CreateAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    void HardDelete(TEntity entity);
}