using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;
using Xipona.Api.Generators.Core.Converters;

namespace Xipona.Api.Repositories.Generators.Converters;

[Generator]
public class ToDomainConverterDiGenerator : ConverterDiGeneratorBase
{
    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var converters = GetAllConverters(context, "IToDomainConverter",
            "Xipona.Api.Repositories");

        context.RegisterSourceOutput(converters.Collect(), (ctx, allConvertersArrays) =>
        {
            var allConverters = allConvertersArrays.SelectMany(x => x).ToImmutableArray();

            if (allConverters.Length == 0)
                throw new InvalidOperationException("No domain converters detected");

            var src = $$"""
                        using Microsoft.Extensions.DependencyInjection;
                        using Xipona.Api.Core.Converter;
                        using System;

                        namespace Xipona.Api.Repositories;

                        public static class RepositoriesToDomainConverterServiceCollectionExtensions
                        {
                            public static IServiceCollection AddToDomainConverter(this IServiceCollection services)
                            {
                                {{GetRegistrations(allConverters, "IToDomainConverter")}}
                                return services;
                            }
                        }
                        """;

            ctx.AddSource("ServiceCollectionExtensions.g.cs", SourceText.From(src, Encoding.UTF8));
        });
    }
}
