using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace Xipona.Api.Generators.Converters.Repositories;

[Generator]
public class RepositoriesToContractConverterDiGenerator : ConverterDiGeneratorBase
{
    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var converters = GetAllConverters(context, "IToContractConverter",
            "ProjectHermes.Xipona.Api.Repositories");

        context.RegisterSourceOutput(converters.Collect(), (ctx, allConvertersArrays) =>
        {
            var allConverters = allConvertersArrays.SelectMany(x => x).ToImmutableArray();

            if (allConverters.Length == 0)
                return;

            var src = $$"""
                        using Microsoft.Extensions.DependencyInjection;
                        using ProjectHermes.Xipona.Api.Core.Converter;
                        using System;

                        namespace ProjectHermes.Xipona.Api.Repositories;

                        public static class RepositoriesToContractConverterServiceCollectionExtensions
                        {
                            public static IServiceCollection AddToContractConverter(this IServiceCollection services)
                            {
                                {{GetRegistrations(allConverters, "IToContractConverter")}}
                                return services;
                            }
                        }
                        """;

            ctx.AddSource("ServiceCollectionExtensions.g.cs", SourceText.From(src, Encoding.UTF8));
        });
    }
}
