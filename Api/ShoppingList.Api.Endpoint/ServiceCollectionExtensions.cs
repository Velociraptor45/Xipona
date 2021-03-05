using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;

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
    }
}