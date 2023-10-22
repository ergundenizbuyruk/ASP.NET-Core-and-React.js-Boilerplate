using Microsoft.EntityFrameworkCore;
using Pattern.Core.Interfaces;
using Pattern.Persistence.Context;

namespace Pattern.Persistence.Repositories
{
    public class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity> CreateAsync(TEntity Entity)
        {
            var result = await _dbSet.AddAsync(Entity);
            return result.Entity;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsQueryable<TEntity>();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetAsync(TPrimaryKey Id)
        {
            return await _dbSet.FindAsync(Id);
        }

        public TEntity Update(TEntity Entity)
        {
            var result = _dbSet.Update(Entity);
            return result.Entity;
        }

        public void Delete(TEntity Entity)
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                (Entity as ISoftDelete).IsDeleted = true;

                if (typeof(IFullAudited).IsAssignableFrom(typeof(TEntity)))
                {
                    (Entity as IFullAudited).DeletionTime = DateTime.Now;
                }

                _dbSet.Attach(Entity);
                _context.Entry(Entity).State = EntityState.Modified;
            }
            else
            {
                _dbSet.Remove(Entity);
            }
        }

        public void HardDelete(TEntity Entity)
        {
            _dbSet.Remove(Entity);
        }
    }
}
