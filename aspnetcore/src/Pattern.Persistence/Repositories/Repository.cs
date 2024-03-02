using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pattern.Core.Interfaces;
using Pattern.Persistence.Context;
using System.Security.Claims;

namespace Pattern.Persistence.Repositories
{
	public class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class
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
			var softDeleteEntity = Entity as ISoftDelete;
			if (softDeleteEntity != null)
			{
				Guid? userId = null;
				var userIdStr = httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userIdStr != null)
				{
					userId = Guid.Parse(userIdStr);
				}

				softDeleteEntity.IsDeleted = true;
				softDeleteEntity.DeletionTime = DateTimeOffset.UtcNow;
				softDeleteEntity.DeleterUserId = userId;

				dbSet.Attach(Entity);
				context.Entry(Entity).State = EntityState.Modified;
				return;
			}
			dbSet.Remove(Entity);
		}

		public void HardDelete(TEntity Entity)
		{
			dbSet.Remove(Entity);
		}
	}
}
