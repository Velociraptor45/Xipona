using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddImplementationOfGenericType(this IServiceCollection services, Assembly assembly,
        Type type)
    {
        var assemblyTypes = assembly
            .GetTypes()
            .Where(t => !t.IsAbstract)
            .ToList();
        foreach (var assemblyType in assemblyTypes)
        {
            var interfaceTypes = assemblyType
                .GetInterfaces()
                .Where(t => t.IsGenericType
                            && t.GetGenericTypeDefinition() == type
                            && services.All(service => !service.TypeIsInDescriptor(t, assemblyType)));

            foreach (var interfaceType in interfaceTypes)
            {
                services.AddTransient(interfaceType, assemblyType);
            }
        }
    }

    public static void AddImplementationOfNonGenericType(this IServiceCollection services, Assembly assembly,
        Type type)
    {
        var assemblyTypes = assembly
            .GetTypes()
            .Where(t => !t.IsAbstract)
            .ToList();

        foreach (var assemblyType in assemblyTypes)
        {
            var interfaceTypes = assemblyType
                .GetInterfaces()
                .Where(t => !t.IsGenericType
                            && t == type
                            && services.All(service => !service.TypeIsInDescriptor(t, assemblyType)));

            foreach (var interfaceType in interfaceTypes)
            {
                services.AddTransient(interfaceType, assemblyType);
            }
        }
    }

    public static bool TypeIsInDescriptor(this ServiceDescriptor descriptor, Type serviceType, Type implementationType)
    {
        if (descriptor.ServiceType != serviceType)
            return false;

        if (descriptor.ImplementationType == implementationType
            || descriptor.ImplementationInstance != null
            && descriptor.ImplementationInstance.GetType() == implementationType)
            return true;

        return false;
    }
}