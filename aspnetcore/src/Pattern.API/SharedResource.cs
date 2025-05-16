using System.Reflection;
using Microsoft.Extensions.Localization;

namespace Pattern.API;

public class SharedResource
{
}

public interface IResourceLocalizer
{
    public string Localize(string key);
}

public class ResourceLocalizer : IResourceLocalizer
{
    private readonly IStringLocalizer localizer;

    public ResourceLocalizer(IStringLocalizerFactory factory)
    {
        var type = typeof(SharedResource);
        var assemblyName = new AssemblyName(type.Assembly.FullName!);
        localizer = factory.Create("SharedResource", assemblyName.Name!);
    }

    public string Localize(string key)
    {
        return localizer[key];
    }
}