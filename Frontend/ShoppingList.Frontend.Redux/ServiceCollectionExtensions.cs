using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Frontend.Redux;
public static class ServiceCollectionExtensions
{
    public static void AddRedux(this IServiceCollection services)
    {
        services.AddFluxor(config =>
        {
            config
                .ScanAssemblies(Assembly.GetExecutingAssembly())
                .UseReduxDevTools(); //todo change only to dev
        });
    }
}
