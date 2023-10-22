using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pattern.Core.Entites.Authentication;
using Pattern.Core.Entites.BaseEntity;
using Pattern.Core.Interfaces;
using System.Linq.Expressions;
using System.Security.AccessControl;

namespace Pattern.Persistence.Context
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "p");
                    var deletedCheck = Expression.Lambda(Expression.Equal(Expression.Property(parameter, "IsDeleted"), Expression.Constant(false)), parameter);
                    builder.Entity(entityType.ClrType).HasQueryFilter(deletedCheck);
                }
            }
			base.OnModelCreating(builder);
		}

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var insertedEntries = ChangeTracker.Entries().Where(x => x.State == EntityState.Added).Select(x => x.Entity);
            foreach (var insertedEntry in insertedEntries)
            {
                var auditableEntity = insertedEntry as IFullAudited;
                if (auditableEntity != null)
                {
                    auditableEntity.CreationTime = DateTime.Now;
                }
            }

            var modifiedEntries = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).Select(x => x.Entity);
            foreach (var modifiedEntry in modifiedEntries)
            {
                var auditableEntity = modifiedEntry as IFullAudited;
                if (auditableEntity != null)
                {
                    auditableEntity.LastModificationTime = DateTime.Now;
                }
            }

            var deletedEntries = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).Select(x => x.Entity);
            foreach (var deletedEntry in deletedEntries)
            {
                var auditableEntity = deletedEntry as IFullAudited;
                if (auditableEntity != null)
                {
                    auditableEntity.DeletionTime = DateTime.Now;
                }

				var softDeleteEntity = deletedEntry as ISoftDelete;
				if (softDeleteEntity != null)
				{
					softDeleteEntity.IsDeleted = true;
					this.Entry(softDeleteEntity).State = EntityState.Modified;
				}
			}

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
