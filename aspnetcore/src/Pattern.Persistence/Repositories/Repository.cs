using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pattern.Core.Interfaces;
using Pattern.Persistence.Context;
using System.Security.Claims;
using Pattern.Core.Entites.BaseEntity;

namespace Pattern.Persistence.Repositories;

public class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
    where TEntity : class, IEntity<TPrimaryKey>
{
    private readonly ApplicationDbContext context;
    private readonly DbSet<TEntity> dbSet;
    private readonly IHttpContextAccessor httpContextAccessor;

    public Repository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        this.context = context;
        dbSet = this.context.Set<TEntity>();
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task CreateAsync(TEntity entity)
    {
        await dbSet.AddAsync(entity);
    }

    public IQueryable<TEntity> GetAll()
    {
        return dbSet.AsQueryable<TEntity>();
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await dbSet.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TPrimaryKey key)
    {
        return await dbSet.FindAsync(key);
    }

    public void Update(TEntity entity)
    {
        dbSet.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        if (entity is ISoftDelete softDeleteEntity)
        {
            Guid? userId = null;
            var userIdStr = httpContextAccessor?.HttpContext?.User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdStr != null)
            {
                userId = Guid.Parse(userIdStr);
            }

            softDeleteEntity.IsDeleted = true;
            softDeleteEntity.DeletionTime = DateTimeOffset.UtcNow;
            softDeleteEntity.DeleterUserId = userId;

            context.Entry(entity).State = EntityState.Modified;
            return;
        }

        dbSet.Remove(entity);
    }

    public void HardDelete(TEntity entity)
    {
        dbSet.Remove(entity);
    }
}