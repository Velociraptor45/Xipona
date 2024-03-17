using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters
{
    internal static class ServiceCollectionExtensions
    {
        internal static void AddConverters(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddImplementationOfGenericType(assembly, typeof(IToContractConverter<,>));
            services.AddImplementationOfGenericType(assembly, typeof(IToDomainConverter<,>));

            services.AddSingleton<IApiConverters>(provider =>
            {
                var toContract = new ApiToContractConverters();
                var toDomain = new ApiToDomainConverters();

                AddConverters(provider, toContract, Assembly.GetExecutingAssembly(), typeof(IToContractConverter<,>));
                AddConverters(provider, toDomain, Assembly.GetExecutingAssembly(), typeof(IToDomainConverter<,>));
                return new ApiConverters(toDomain, toContract);
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
                var interfaceType = assemblyType
                    .GetInterfaces()
                    .SingleOrDefault(t => t.IsGenericType
                                          && t.GetGenericTypeDefinition() == type);

                if (interfaceType == null)
                    continue;

                var args = interfaceType.GetGenericArguments();
                var implementation = provider.GetRequiredService(interfaceType) as TConverter;
                if (implementation == null || args.Length != 2)
                    continue;

                dict.Add((args[0], args[1]), implementation);
            }
        }
    }
}