using Microsoft.Extensions.Logging;
using Xipona.Api.Core.DomainEventHandlers;
using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.ShoppingLists.Services.Deletions;
using Xipona.Api.Domain.Stores.DomainEvents;

namespace Xipona.Api.Domain.ShoppingLists.EventHandlers;

public class StoreDeletedDomainEventHandler : IDomainEventHandler<StoreDeletedDomainEvent>
{
    private readonly Func<CancellationToken, IShoppingListDeletionService> _shoppingListDeletionServiceDelegate;
    private readonly ILogger<StoreDeletedDomainEventHandler> _logger;

    public StoreDeletedDomainEventHandler(
        Func<CancellationToken, IShoppingListDeletionService> shoppingListDeletionServiceDelegate,
        ILogger<StoreDeletedDomainEventHandler> logger)
    {
        _shoppingListDeletionServiceDelegate = shoppingListDeletionServiceDelegate;
        _logger = logger;
    }

    public async Task HandleAsync(StoreDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Started handling {EventName} for shopping lists", nameof(StoreDeletedDomainEvent));

        var service = _shoppingListDeletionServiceDelegate(cancellationToken);
        await service.HardDeleteForStoreAsync(domainEvent.StoreId);

        _logger.LogDebug("Finished handling {EventName} for shopping lists", nameof(StoreDeletedDomainEvent));
    }
}