using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Xipona.Api.ApplicationServices.Generators.CqrsHandlers;

[Generator]
public class CommandHandlerDiGenerator : CqrsHandlerDiGeneratorBase
{
    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var commandHandlers = GetAllHandlers(context, "ICommandHandler");

        context.RegisterSourceOutput(commandHandlers.Collect(), (ctx, allCommandHandlers) =>
        {
            if (allCommandHandlers.Length == 0)
                throw new InvalidOperationException("No command handlers detected");

            var src = $$"""
                        using Microsoft.Extensions.DependencyInjection;
                        using Xipona.Api.ApplicationServices.Common.Commands;
                        using System;

                        namespace Xipona.Api.ApplicationServices;

                        public static class CommandHandlerServiceCollectionExtensions
                        {
                            public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
                            {
                                {{GetRegistrations(allCommandHandlers, "ICommandHandler")}}
                                
                                return services;
                            }
                        }
                        """;

            ctx.AddSource("SCE.g.cs", SourceText.From(src, Encoding.UTF8));
        });
    }
}