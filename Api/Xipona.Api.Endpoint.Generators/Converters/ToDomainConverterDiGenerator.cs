using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;
using Xipona.Api.Generators.Core.Converters;

namespace Xipona.Api.Endpoint.Generators.Converters;

[Generator]
public class ToDomainConverterDiGenerator : ConverterDiGeneratorBase
{
    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var converters = GetAllConverters(context, "IToDomainConverter",
            "ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain");

        context.RegisterSourceOutput(converters.Collect(), (ctx, allConvertersArrays) =>
        {
            var allConverters = allConvertersArrays.SelectMany(x => x).ToImmutableArray();

            if (allConverters.Length == 0)
                throw new InvalidOperationException("No domain converters detected");

            var src = $$"""
                        using Microsoft.Extensions.DependencyInjection;
                        using ProjectHermes.Xipona.Api.Core.Converter;
                        using System;

                        namespace ProjectHermes.Xipona.Api.Endpoint;

                        public static class EndpointToDomainConverterServiceCollectionExtensions
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
