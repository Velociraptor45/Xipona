using Microsoft.Extensions.DependencyInjection;
using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.ApplicationServices.Common.Events;
using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Core.DomainEventHandlers;

namespace Xipona.Api.ApplicationServices;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IQueryDispatcher, QueryDispatcher>();
        services.AddTransient<ICommandDispatcher, CommandDispatcher>();
        services.AddTransient<Func<CancellationToken, IDomainEventDispatcher>>(provider =>
        {
            return cancellationToken => new DomainEventDispatcher(provider, cancellationToken);
        });

        services.AddQueryHandlers();
        services.AddCommandHandlers();
    }
}