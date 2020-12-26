using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IQueryDispatcher, QueryDispatcher>();
            serviceCollection.AddTransient<ICommandDispatcher, CommandDispatcher>();
        }
    }
}