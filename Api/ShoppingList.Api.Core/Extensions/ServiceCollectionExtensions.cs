using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddConverters(this IServiceCollection services, Assembly assembly,
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
                        && services.All(service => !IsTypeIsInDescriptor(service, t, assemblyType)));

                foreach (var interfaceType in interfaceTypes)
                {
                    services.AddTransient(interfaceType, assemblyType);
                }
            }
        }

        private static bool IsTypeIsInDescriptor(ServiceDescriptor descriptor, Type serviceType, Type implementationType)
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
}