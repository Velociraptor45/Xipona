using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Xipona.Api.Generators.Handlers;

[Generator]
public class QueryHandlerDiGenerator : HandlerDiGeneratorBase
{
    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var queryHandlers = GetAllHandlers(context, "IQueryHandler");

        context.RegisterSourceOutput(queryHandlers.Collect(), (ctx, allQueryHandlers) =>
        {
            if (allQueryHandlers.Length == 0)
                return;

            var src = $$"""
                        {{GetNamespaces(allQueryHandlers, "ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries")}}

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

            ctx.AddSource("QueryHandlerServiceCollectionExtensions.g.cs", SourceText.From(src, Encoding.UTF8));
        });
    }
}