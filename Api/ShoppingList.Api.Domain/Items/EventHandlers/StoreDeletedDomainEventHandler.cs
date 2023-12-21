using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Stores.DomainEvents;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.EventHandlers;

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
        _logger.LogDebug(() => "Started handling {EventName} for items", nameof(StoreDeletedDomainEvent));

        var service = _itemModificationServiceDelegate(cancellationToken);
        await service.RemoveAvailabilitiesForAsync(domainEvent.StoreId);

        _logger.LogDebug(() => "Finished handling {EventName} for items", nameof(StoreDeletedDomainEvent));
    }
}