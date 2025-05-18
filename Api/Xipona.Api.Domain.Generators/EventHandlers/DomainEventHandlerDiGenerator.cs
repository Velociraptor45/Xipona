using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Xipona.Api.Domain.Generators.EventHandlers;

[Generator]
public class DomainEventHandlerDiGenerator : EventHandlerDiGeneratorBase
{
    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var handlers = GetAllHandlers(context, "IDomainEventHandler");

        context.RegisterSourceOutput(handlers.Collect(), (ctx, allHandlers) =>
        {
            if (allHandlers.Length == 0)
                throw new InvalidOperationException("No domain event handlers detected");

            var src = $$"""
                        using Microsoft.Extensions.DependencyInjection;
                        using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
                        using System;

                        namespace ProjectHermes.Xipona.Api.Domain;

                        public static class DomainEventHandlerServiceCollectionExtensions
                        {
                            public static IServiceCollection AddDomainEventHandlers(this IServiceCollection services)
                            {
                                {{GetRegistrations(allHandlers, "IDomainEventHandler")}}
                                
                                return services;
                            }
                        }
                        """;

            ctx.AddSource("ServiceCollectionExtensions.g.cs", SourceText.From(src, Encoding.UTF8));
        });
    }
}