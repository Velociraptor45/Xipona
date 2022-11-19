using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.RequestSenders;
using System;
using System.Linq;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddConverters();
            AddImplementationOfType(services, Assembly.GetExecutingAssembly(), typeof(IRequestSender));
            services.AddTransient<IRequestSenderStrategy, RequestSenderStrategy>();
        }

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

        public static void AddImplementationOfType(this IServiceCollection services, Assembly assembly,
            Type type)
        {
            var assemblyTypes = assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && t.GetInterfaces().Any(i => i == type))
                .ToList();

            foreach (var assemblyType in assemblyTypes)
            {
                services.AddTransient(type, assemblyType);
            }
        }

        private static bool TypeIsInDescriptor(this ServiceDescriptor descriptor, Type serviceType,
            Type implementationType)
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