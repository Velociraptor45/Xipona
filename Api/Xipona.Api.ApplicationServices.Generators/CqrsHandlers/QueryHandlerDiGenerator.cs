using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Xipona.Api.ApplicationServices.Generators.CqrsHandlers;

[Generator]
public class QueryHandlerDiGenerator : CqrsHandlerDiGeneratorBase
{
    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var queryHandlers = GetAllHandlers(context, "IQueryHandler");

        context.RegisterSourceOutput(queryHandlers.Collect(), (ctx, allQueryHandlers) =>
        {
            if (allQueryHandlers.Length == 0)
                throw new InvalidOperationException("No query handlers detected");

            var src = $$"""
                        using Microsoft.Extensions.DependencyInjection;
                        using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
                        using System;

                        namespace ProjectHermes.Xipona.Api.ApplicationServices;

                        public static class QueryHandlerServiceCollectionExtensions
                        {
                            public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
                            {
                                {{GetRegistrations(allQueryHandlers, "IQueryHandler")}}
                                
                                return services;
                            }
                        }
                        """;

            ctx.AddSource("SCE.g.cs", SourceText.From(src, Encoding.UTF8));
        });
    }
}