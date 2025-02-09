using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoint.v1.Converters;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoint;

public static class ServiceCollectionExtensions
{
    public static void AddEndpointControllers(this IServiceCollection services)
    {
        services.AddTransient<ShoppingListController>();
        services.AddTransient<StoreController>();
        services.AddTransient<RecipeTagController>();

        var assembly = typeof(ServiceCollectionExtensions).Assembly;
        services.AddImplementationOfGenericType(assembly, typeof(IToContractConverter<,>));
        services.AddImplementationOfGenericType(assembly, typeof(IToDomainConverter<,>));

        services.AddSingleton<IEndpointConverters>(provider =>
        {
            var toContract = new EndpointToContractConverters();
            var toDomain = new EndpointToDomainConverters();

            AddConverters(provider, toContract, Assembly.GetExecutingAssembly(), typeof(IToContractConverter<,>));
            AddConverters(provider, toDomain, Assembly.GetExecutingAssembly(), typeof(IToDomainConverter<,>));
            return new EndpointConverters(toDomain, toContract);
        });
    }

    private static void AddConverters<TConverter>(IServiceProvider provider,
        Dictionary<(Type, Type), TConverter> dict, Assembly assembly, Type type)
        where TConverter : class, IConverter
    {
        var assemblyTypes = assembly
            .GetTypes()
            .Where(t => !t.IsAbstract)
            .ToList();
        foreach (var assemblyType in assemblyTypes)
        {
            var interfaceTypes = assemblyType
                .GetInterfaces()
                .Where(t => t.IsGenericType
                                      && t.GetGenericTypeDefinition() == type);

            foreach (var interfaceType in interfaceTypes)
            {
                var args = interfaceType.GetGenericArguments();
                if (provider.GetRequiredService(interfaceType) is not TConverter implementation || args.Length != 2)
                    continue;

                dict.Add((args[0], args[1]), implementation);
            }
        }
    }
}