using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Endpoint.V1.Controllers;

namespace ShoppingList.Endpoint
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEndpointControllers(this IServiceCollection services)
        {
            services.AddTransient<ShoppingListController>();
        }
    }
}