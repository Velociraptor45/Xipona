using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.EventHandlers;

public class ItemDeletedDomainEventHandler : IDomainEventHandler<ItemDeletedDomainEvent>
{
    private readonly Func<CancellationToken, IShoppingListModificationService> _shoppingListModificationServiceDelegate;

    public ItemDeletedDomainEventHandler(
        Func<CancellationToken, IShoppingListModificationService> shoppingListModificationServiceDelegate)
    {
        _shoppingListModificationServiceDelegate = shoppingListModificationServiceDelegate;
    }

    public async Task HandleAsync(ItemDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var service = _shoppingListModificationServiceDelegate(cancellationToken);
        await service.RemoveItemAndItsTypesFromCurrentListAsync(domainEvent.ItemId);
    }
}