using Pattern.Application.Services.Base;
using Pattern.Application.Services.Emails;
using Pattern.Persistence.UnitOfWork;
using System.Reflection;

namespace Pattern.API.Extensions
{
	public static class AddServicesExtension
	{
		public static void AddServices(this IServiceCollection services, Assembly assembly)
		{
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IEmailSender, EmailSender>();

			var appServiceClassType = typeof(ApplicationService);
			var appServiceInterfaceType = typeof(IApplicationService);
			var crudServiceInterfaceType = typeof(ICrudService<,,,,>);

			// Get all service class types
			var allServiceClassTypes = assembly.GetTypes().Where(type => type.IsClass && !type.IsAbstract &&
				type.BaseType != null && appServiceClassType.IsAssignableFrom(type));

			// Get all service interface types
			var allServiceInterfaceTypes = assembly.GetTypes().Where(type => type.IsInterface && appServiceInterfaceType.IsAssignableFrom(type) &&
				type != appServiceInterfaceType && type != crudServiceInterfaceType);

			// add scoped
			foreach (var TServiceClass in allServiceClassTypes)
			{
				var interfaceTypes = allServiceInterfaceTypes.Where(serviceInterface => serviceInterface.IsAssignableFrom(TServiceClass)).ToList();

				if (interfaceTypes.Count != 1)
				{
					throw new InvalidOperationException($"Service '{nameof(TServiceClass)}' must implement only one interface that implements ICrudService<,,,,> or IApplicationService");
				}

				services.AddScoped(interfaceTypes[0], TServiceClass);
			}
		}
	}
}
