using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace Xipona.Api.Generators.Converters.Endpoints;

[Generator]
public class EndpointToContractConverterDiGenerator : ConverterDiGeneratorBase
{
    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var converters = GetAllConverters(context, "IToContractConverter",
            "ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract");

        context.RegisterSourceOutput(converters.Collect(), (ctx, allConvertersArrays) =>
        {
            var allConverters = allConvertersArrays.SelectMany(x => x).ToImmutableArray();

            if (allConverters.Length == 0)
                return;

            var src = $$"""
                        using Microsoft.Extensions.DependencyInjection;
                        using ProjectHermes.Xipona.Api.Core.Converter;
                        using System;

                        namespace ProjectHermes.Xipona.Api.Endpoint;

                        public static class EndpointToContractConverterServiceCollectionExtensions
                        {
                            public static IServiceCollection AddToContractConverter(this IServiceCollection services)
                            {
                                {{GetRegistrations(allConverters, "IToContractConverter")}}
                                return services;
                            }
                        }
                        """;

            ctx.AddSource("EndpointToContractServiceCollectionExtensions.g.cs", SourceText.From(src, Encoding.UTF8));
        });
    }
}
