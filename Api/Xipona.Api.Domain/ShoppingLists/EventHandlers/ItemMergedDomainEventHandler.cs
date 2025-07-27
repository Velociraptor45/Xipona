using Xipona.Api.Core.DomainEventHandlers;
using Xipona.Api.Domain.Items.DomainEvents;
using Xipona.Api.Domain.ShoppingLists.Services.Modifications;

namespace Xipona.Api.Domain.ShoppingLists.EventHandlers;

public class ItemMergedDomainEventHandler : IDomainEventHandler<ItemMergedDomainEvent>
{
    private readonly Func<CancellationToken, IShoppingListModificationService> _shoppingListModificationServiceDelegate;

    public ItemMergedDomainEventHandler(
        Func<CancellationToken, IShoppingListModificationService> shoppingListModificationServiceDelegate)
    {
        _shoppingListModificationServiceDelegate = shoppingListModificationServiceDelegate;
    }

    public async Task HandleAsync(ItemMergedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var service = _shoppingListModificationServiceDelegate(cancellationToken);
        await service.ReplaceMergedItemAsync(domainEvent.OriginalItemId, domainEvent.NewItemId, domainEvent.NewItemTypeId);
    }
}