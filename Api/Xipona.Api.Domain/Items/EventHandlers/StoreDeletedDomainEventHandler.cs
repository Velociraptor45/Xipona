using Microsoft.Extensions.Logging;
using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.Stores.DomainEvents;

namespace ProjectHermes.Xipona.Api.Domain.Items.EventHandlers;

public class StoreDeletedDomainEventHandler : IDomainEventHandler<StoreDeletedDomainEvent>
{
    private readonly Func<CancellationToken, IItemModificationService> _itemModificationServiceDelegate;
    private readonly ILogger<StoreDeletedDomainEventHandler> _logger;

    public StoreDeletedDomainEventHandler(
        Func<CancellationToken, IItemModificationService> itemModificationServiceDelegate,
        ILogger<StoreDeletedDomainEventHandler> logger)
    {
        _itemModificationServiceDelegate = itemModificationServiceDelegate;
        _logger = logger;
    }

    public async Task HandleAsync(StoreDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Started handling {EventName} for items", nameof(StoreDeletedDomainEvent));

        var service = _itemModificationServiceDelegate(cancellationToken);
        await service.RemoveAvailabilitiesForAsync(domainEvent.StoreId);

        _logger.LogDebug("Finished handling {EventName} for items", nameof(StoreDeletedDomainEvent));
    }
}