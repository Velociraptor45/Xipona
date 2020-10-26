using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Api.Endpoint.v1.Controllers;

namespace ShoppingList.Endpoint
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
        }
    }
}