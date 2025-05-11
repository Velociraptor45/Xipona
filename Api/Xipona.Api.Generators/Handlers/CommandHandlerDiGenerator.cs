using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Xipona.Api.Generators.Handlers;

[Generator]
public class CommandHandlerDiGenerator : HandlerDiGeneratorBase
{
    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var commandHandlers = GetAllHandlers(context, "ICommandHandler");

        context.RegisterSourceOutput(commandHandlers.Collect(), (ctx, allCommandHandlers) =>
        {
            if (allCommandHandlers.Length == 0)
                return;

            var src = $$"""
                        {{GetNamespaces(allCommandHandlers, "ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands")}}

                        namespace ProjectHermes.Xipona.Api.ApplicationServices;

                        public static class CommandHandlerServiceCollectionExtensions
                        {
                            public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
                            {
                                {{GetRegistrations(allCommandHandlers, "ICommandHandler")}}
                                
                                return services;
                            }
                        }
                        """;

            ctx.AddSource("CommandHandlerServiceCollectionExtensions.g.cs", SourceText.From(src, Encoding.UTF8));
        });
    }
}