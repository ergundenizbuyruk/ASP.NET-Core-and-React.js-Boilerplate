using Microsoft.EntityFrameworkCore;
using Pattern.Core.Interfaces;
using Pattern.Persistence.Context;

namespace Pattern.Persistence.Repositories
{
	public class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class
	{
		private readonly ApplicationDbContext context;
		private readonly DbSet<TEntity> dbSet;
		public Repository(ApplicationDbContext context)
		{
			this.context = context;
			dbSet = this.context.Set<TEntity>();
		}

		public async Task<TEntity> CreateAsync(TEntity Entity)
		{
			var result = await dbSet.AddAsync(Entity);
			return result.Entity;
		}

		public IQueryable<TEntity> GetAll()
		{
			return dbSet.AsQueryable<TEntity>();
		}

		public async Task<List<TEntity>> GetAllAsync(int? pageNumber, int? pageSize)
		{
			if (pageNumber is null || pageSize is null)
			{
				return await dbSet.ToListAsync();
			}

			int skipAmount = ((int)pageNumber - 1) * (int)pageSize;

			return await dbSet
				.Skip(skipAmount)
				.Take((int)pageSize)
				.ToListAsync();
		}

		public async Task<TEntity> GetByIdAsync(TPrimaryKey Id)
		{
			return await dbSet.FindAsync(Id);
		}

		public TEntity Update(TEntity Entity)
		{
			var result = dbSet.Update(Entity);
			return result.Entity;
		}

		public TEntity SetValuesAndUpdate(TEntity EntityFromDb, TEntity EntityFromDto)
		{
			dbSet.Entry(EntityFromDb).CurrentValues.SetValues(EntityFromDto);
			return Update(EntityFromDb);
		}

		public void Delete(TEntity Entity)
		{
			if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
			{
				(Entity as ISoftDelete).IsDeleted = true;

				if (typeof(IFullAudited).IsAssignableFrom(typeof(TEntity)))
				{
					(Entity as IFullAudited).DeletionTime = DateTimeOffset.UtcNow;
				}

				dbSet.Attach(Entity);
				context.Entry(Entity).State = EntityState.Modified;
			}
			else
			{
				dbSet.Remove(Entity);
			}
		}

		public void HardDelete(TEntity Entity)
		{
			dbSet.Remove(Entity);
		}
	}
}
