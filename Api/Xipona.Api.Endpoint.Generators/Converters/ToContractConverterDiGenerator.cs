using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;
using Xipona.Api.Generators.Core.Converters;

namespace Xipona.Api.Endpoint.Generators.Converters;

[Generator]
public class ToContractConverterDiGenerator : ConverterDiGeneratorBase
{
    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var converters = GetAllConverters(context, "IToContractConverter",
            "Xipona.Api.Endpoint.v1.Converters.ToContract");

        context.RegisterSourceOutput(converters.Collect(), (ctx, allConvertersArrays) =>
        {
            var allConverters = allConvertersArrays.SelectMany(x => x).ToImmutableArray();

            if (allConverters.Length == 0)
                throw new InvalidOperationException("No contract converters detected");

            var src = $$"""
                        using Microsoft.Extensions.DependencyInjection;
                        using Xipona.Api.Core.Converter;
                        using System;

                        namespace Xipona.Api.Endpoint;

                        public static class EndpointToContractConverterServiceCollectionExtensions
                        {
                            public static IServiceCollection AddToContractConverter(this IServiceCollection services)
                            {
                                {{GetRegistrations(allConverters, "IToContractConverter")}}
                                return services;
                            }
                        }
                        """;

            ctx.AddSource("SCE.g.cs", SourceText.From(src, Encoding.UTF8));
        });
    }
}
