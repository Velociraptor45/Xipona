using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using System;
using System.Linq;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Domain
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDomain(this IServiceCollection services)
        {
            services.AddHandlersForAssembly(typeof(ServiceCollectionExtensions).Assembly);
        }

        public static void AddHandlersForAssembly(this IServiceCollection services, Assembly assembly)
        {
            services.AddQueryHandlersForAssembly(assembly);
            services.AddCommandHandlersForAssembly(assembly);
        }

        public static void AddQueryHandlersForAssembly(this IServiceCollection services, Assembly assembly)
        {
            var handlerType = typeof(IQueryHandler<,>);
            services.AddHandlersForAssembly(assembly, handlerType);
        }

        public static void AddCommandHandlersForAssembly(this IServiceCollection services, Assembly assembly)
        {
            var handlerType = typeof(ICommandHandler<,>);
            services.AddHandlersForAssembly(assembly, handlerType);
        }

        private static void AddHandlersForAssembly(this IServiceCollection services, Assembly assembly,
            Type handlerType)
        {
            var assemblyTypes = assembly
                .GetTypes()
                .Where(t => !t.IsAbstract)
                .ToList();

            foreach (var assemblyType in assemblyTypes)
            {
                var interfaceTypes = assemblyType
                    .GetInterfaces()
                    .Where(type => type.IsGenericType
                        && type.GetGenericTypeDefinition() == handlerType
                        && services.All(service => !IsTypeIsInDescriptor(service, type, assemblyType)));

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