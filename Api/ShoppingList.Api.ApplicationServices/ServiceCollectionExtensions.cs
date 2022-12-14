using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Events;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IQueryDispatcher, QueryDispatcher>();
        serviceCollection.AddTransient<ICommandDispatcher, CommandDispatcher>();
        serviceCollection.AddTransient<Func<CancellationToken, IDomainEventDispatcher>>(provider =>
        {
            return cancellationToken => new DomainEventDispatcher(provider, cancellationToken);
        });

        serviceCollection.AddHandlersForAssembly(Assembly.GetExecutingAssembly());
    }

    private static void AddHandlersForAssembly(this IServiceCollection services, Assembly assembly)
    {
        services.AddImplementationOfGenericType(assembly, typeof(IQueryHandler<,>));
        services.AddImplementationOfGenericType(assembly, typeof(ICommandHandler<,>));
    }
}