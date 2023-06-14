using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Deletions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.DomainEvents;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.EventHandlers;

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
        _logger.LogDebug(() => $"Started handling {nameof(StoreDeletedDomainEvent)} for shopping lists");

        var service = _shoppingListDeletionServiceDelegate(cancellationToken);
        await service.HardDeleteForStoreAsync(domainEvent.StoreId);

        _logger.LogDebug(() => $"Finished handling {nameof(StoreDeletedDomainEvent)} for shopping lists");
    }
}