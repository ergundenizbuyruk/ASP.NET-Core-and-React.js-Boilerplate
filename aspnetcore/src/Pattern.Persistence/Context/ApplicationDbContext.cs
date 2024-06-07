using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pattern.Core.Entites;
using Pattern.Core.Entites.Authentication;
using Pattern.Core.Interfaces;
using Pattern.Core.Options;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text.Json;

namespace Pattern.Persistence.Context
{
	public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
	{
		private readonly IHttpContextAccessor httpContextAccessor;
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
		{
			this.httpContextAccessor = httpContextAccessor;
		}
		public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
		public DbSet<Permission> Permissions { get; set; }
		public DbSet<RolePermission> RolePermissions { get; set; }
		public DbSet<Province> Provinces { get; set; }
		public DbSet<District> Districts { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// changed the default table names
			builder.Entity<User>().ToTable("Users");
			builder.Entity<Role>().ToTable("Roles");
			builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
			builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
			builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
			builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
			builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
			builder.Entity<Province>().ToTable("Provinces");
			builder.Entity<District>().ToTable("Districts");


			foreach (var entityType in builder.Model.GetEntityTypes())
			{
				if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
				{
					var parameter = Expression.Parameter(entityType.ClrType, "p");
					var deletedCheck = Expression.Lambda(Expression.Equal(Expression.Property(parameter, "IsDeleted"), Expression.Constant(false)), parameter);
					builder.Entity(entityType.ClrType).HasQueryFilter(deletedCheck);
				}
			}

			// Configure RolePermission many-to-many relationship
			builder.Entity<Role>()
				.HasMany(r => r.Permissions).WithMany(p => p.Roles)
				.UsingEntity<RolePermission>();

			builder.Entity<RolePermission>()
				.HasKey(rp => new { rp.RoleId, rp.PermissionId });

			// Add all permissions to database
			IEnumerable<Permission> permissions = Enum.GetValues<Core.Enums.Permission>()
				.Select(p => new Permission
				{
					Id = (int)p,
					Name = p.ToString()
				});

			builder.Entity<Permission>().HasData(permissions);

			var hasher = new PasswordHasher<User>();
			var user = new User
			{
				Id = Guid.NewGuid(),
				UserName = "admin",
				NormalizedUserName = "ADMIN",
				Email = "admin@admin.com",
				NormalizedEmail = "admin@admin.com".Normalize().ToUpper(),
				FirstName = "Admin",
				LastName = "Admin",
				IsActive = true,
				EmailConfirmed = true,
				ConcurrencyStamp = Guid.NewGuid().ToString(),
				SecurityStamp = Guid.NewGuid().ToString(),
				PasswordHash = hasher.HashPassword(null, "123qwe")
			};

			var role = new Role
			{
				Id = Guid.NewGuid(),
				Name = "Admin",
				NormalizedName = "ADMIN",
				ConcurrencyStamp = Guid.NewGuid().ToString()
			};

			var userRole = new IdentityUserRole<Guid>
			{
				RoleId = role.Id,
				UserId = user.Id
			};

			IEnumerable<RolePermission> rolePermissions = Enum.GetValues<Core.Enums.Permission>()
				.Select(p => new RolePermission
				{
					RoleId = role.Id,
					PermissionId = (int)p
				});

			builder.Entity<User>().HasData(user);
			builder.Entity<Role>().HasData(role);
			builder.Entity<IdentityUserRole<Guid>>().HasData(userRole);
			builder.Entity<RolePermission>().HasData(rolePermissions);


			string ilJson = File.ReadAllText("iller.json");
			string ilceJson = File.ReadAllText("ilceler.json");
			List<ProvinceJsonModel> iller = JsonSerializer.Deserialize<List<ProvinceJsonModel>>(ilJson);
			List<DistrictJsonModel> ilceler = JsonSerializer.Deserialize<List<DistrictJsonModel>>(ilceJson);

			var provinces = iller.Select(p => new Province
			{
				Id = int.Parse(p.id),
				Plaka = int.Parse(p.plaka),
				ProvinceText = p.il,
				AreaCode = p.alanKodu
			});

			var districts = ilceler.Select(p => new District
			{
				Id = int.Parse(p.id),
				DistrictText = p.ilce,
				ProvinceId = int.Parse(p.ilId)
			});

			builder.Entity<Province>().HasData(provinces);
			builder.Entity<District>().HasData(districts);
		}

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            Guid? userId = null;
            var userIdStr = httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr != null)
            {
                userId = Guid.Parse(userIdStr);
            }

            var insertedEntries = ChangeTracker.Entries().Where(x => x.State == EntityState.Added).Select(x => x.Entity);
            foreach (var insertedEntry in insertedEntries)
            {
                var auditableEntity = insertedEntry as IFullAudited;
                if (auditableEntity != null)
                {
                    auditableEntity.CreationTime = DateTimeOffset.UtcNow;
                    auditableEntity.CreatorUserId = userId;
                }
            }

            var modifiedEntries = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).Select(x => x.Entity);
            foreach (var modifiedEntry in modifiedEntries)
            {
                var auditableEntity = modifiedEntry as IFullAudited;
                if (auditableEntity != null)
                {
                    auditableEntity.LastModificationTime = DateTimeOffset.UtcNow;
                    auditableEntity.LastModifierUserId = userId;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
