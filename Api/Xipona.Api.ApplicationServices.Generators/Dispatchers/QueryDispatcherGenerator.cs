using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Xipona.Api.ApplicationServices.Generators.Dispatchers;

[Generator]
public class QueryDispatcherGenerator : DispatcherGeneratorBase
{
    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var queryHandlers = GetAllHandlers(context, "IQueryHandler");

        context.RegisterSourceOutput(queryHandlers.Collect(), (ctx, allQueryHandlers) =>
        {
            var src = $$"""
                        using Microsoft.Extensions.DependencyInjection;
                        using System;
                        
                        namespace ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
                        
                        public class QueryDispatcher : IQueryDispatcher
                        {
                            private readonly IServiceProvider _serviceProvider;

                            public QueryDispatcher(IServiceProvider serviceProvider)
                            {
                                _serviceProvider = serviceProvider;
                            }

                            public async Task<T> DispatchAsync<T>(IQuery<T> query, CancellationToken cancellationToken)
                            {
                                switch (query)
                                {
                                    {{GetSwitchCases(allQueryHandlers, "IQueryHandler")}}
                                    default:
                                        throw new InvalidOperationException("No handler for query {query.GetType()} registered");
                                }
                            }
                        }
                        """;

            ctx.AddSource("QueryDispatcher.g.cs", SourceText.From(src, Encoding.UTF8));
        });
    }
}
