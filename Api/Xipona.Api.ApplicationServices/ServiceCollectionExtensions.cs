using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Events;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;

namespace ProjectHermes.Xipona.Api.ApplicationServices;

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