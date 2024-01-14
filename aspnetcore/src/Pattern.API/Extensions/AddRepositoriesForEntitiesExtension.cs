using Pattern.Core.Entites.BaseEntity;
using Pattern.Persistence.Repositories;
using System.Reflection;

namespace Pattern.API.Extensions
{
    public static class AddRepositoriesForEntitiesExtension
    {
        public static void AddRepositoriesForEntities(this IServiceCollection services, Assembly assembly)
        {
            var entities = GetEntities(assembly);

            foreach (var entity in entities)
            {
                var primaryKeyType = entity.GetProperty("Id")!.PropertyType;
                var repositoryType = typeof(IRepository<,>).MakeGenericType(entity, primaryKeyType);
                var implementationType = typeof(Repository<,>).MakeGenericType(entity, primaryKeyType);
                services.AddScoped(repositoryType, implementationType);
            }
        }
        private static IEnumerable<Type> GetEntities(Assembly assembly)
        {
            var entityDefinition = typeof(Entity<>).GetGenericTypeDefinition();
            return assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.BaseType is not null &&
                (type.BaseType == typeof(Entity) ||
                type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == entityDefinition));
        }
    }
}
