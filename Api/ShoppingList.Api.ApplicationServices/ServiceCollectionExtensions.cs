using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IQueryDispatcher, QueryDispatcher>();
        serviceCollection.AddTransient<ICommandDispatcher, CommandDispatcher>();

        serviceCollection.AddHandlersForAssembly(Assembly.GetExecutingAssembly());
    }

    private static void AddHandlersForAssembly(this IServiceCollection services, Assembly assembly)
    {
        services.AddQueryHandlersForAssembly(assembly);
        services.AddCommandHandlersForAssembly(assembly);
    }

    private static void AddQueryHandlersForAssembly(this IServiceCollection services, Assembly assembly)
    {
        var handlerType = typeof(IQueryHandler<,>);
        services.AddImplementationOfGenericType(assembly, handlerType);
    }

    private static void AddCommandHandlersForAssembly(this IServiceCollection services, Assembly assembly)
    {
        var handlerType = typeof(ICommandHandler<,>);
        services.AddImplementationOfGenericType(assembly, handlerType);
    }
}