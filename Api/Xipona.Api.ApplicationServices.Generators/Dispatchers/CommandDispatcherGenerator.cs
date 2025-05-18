using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Xipona.Api.ApplicationServices.Generators.Dispatchers;

[Generator]
public class CommandDispatcherGenerator : DispatcherGeneratorBase
{
    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var commandHandlers = GetAllHandlers(context, "ICommandHandler");

        context.RegisterSourceOutput(commandHandlers.Collect(), (ctx, allCommandHandlers) =>
        {
            var src = $$"""
                        using Microsoft.Extensions.DependencyInjection;
                        using System;
                        
                        namespace ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
                        
                        public class CommandDispatcher : ICommandDispatcher
                        {
                            private readonly IServiceProvider _serviceProvider;

                            public CommandDispatcher(IServiceProvider serviceProvider)
                            {
                                _serviceProvider = serviceProvider;
                            }

                            private readonly Dictionary<Type, Type> _typeDict = new()
                            {
                                {{GetDictEntries(allCommandHandlers, "ICommandHandler")}}
                            };

                            public async Task<T> DispatchAsync<T>(ICommand<T> command, CancellationToken cancellationToken)
                            {
                                var commandType = command.GetType();
                                var commandHandlerType = _typeDict[commandType];

                                var commandHandler = _serviceProvider.GetRequiredService(commandHandlerType);
                                return await ((dynamic)commandHandler).HandleAsync((dynamic)command, cancellationToken);
                            }
                        }
                        """;

            ctx.AddSource("CommandDispatcher.g.cs", SourceText.From(src, Encoding.UTF8));
        });
    }
}
