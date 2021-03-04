using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using System;
using System.Linq;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoint
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEndpointControllers(this IServiceCollection services)
        {
            services.AddTransient<ShoppingListController>();
            services.AddTransient<ItemCategoryController>();
            services.AddTransient<ManufacturerController>();
            services.AddTransient<ItemController>();
            services.AddTransient<StoreController>();

            var assembly = typeof(ServiceCollectionExtensions).Assembly;
            services.AddConverters(assembly, typeof(IToContractConverter<,>));
            services.AddConverters(assembly, typeof(IToDomainConverter<,>));
        }

        public static void AddConverters(this IServiceCollection services, Assembly assembly,
            Type converterType)
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
                        && type.GetGenericTypeDefinition() == converterType
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