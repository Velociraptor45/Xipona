using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Xipona.Api.Generators.Converters.Endpoints;

[Generator]
public class EndpointToDomainConverterDiGenerator : ConverterDiGeneratorBase
{
    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var converters = GetAllConverters(context, "IToDomainConverter",
            "ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain");

        context.RegisterSourceOutput(converters.Collect(), (ctx, allConverters) =>
        {
            if (allConverters.Length == 0)
                return;

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

            ctx.AddSource("EndpointToDomainConverterServiceCollectionExtensions.g.cs", SourceText.From(src, Encoding.UTF8));
        });
    }
}
