using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ProjectHermes.Xipona.Frontend.Redux;

public static class ServiceCollectionExtensions
{
    public static void AddRedux(this IServiceCollection services)
    {
        services.AddFluxor(config =>
        {
            config
                .ScanAssemblies(Assembly.GetExecutingAssembly())
#if DEBUG
                .UseReduxDevTools()
#endif
                ;
        });
    }
}