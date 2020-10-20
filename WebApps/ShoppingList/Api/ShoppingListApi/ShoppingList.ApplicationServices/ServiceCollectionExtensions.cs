using Microsoft.Extensions.DependencyInjection;

namespace ShoppingList.ApplicationServices
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IQueryDispatcher, QueryDispatcher>();
        }
    }
}