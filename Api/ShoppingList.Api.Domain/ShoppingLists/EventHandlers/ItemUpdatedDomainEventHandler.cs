using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Exchanges;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.EventHandlers;

public class ItemUpdatedDomainEventHandler : IDomainEventHandler<ItemUpdatedDomainEvent>
{
    private readonly Func<CancellationToken, IShoppingListExchangeService> _shoppingListExchangeServiceDelegate;

    public ItemUpdatedDomainEventHandler(
        Func<CancellationToken, IShoppingListExchangeService> shoppingListExchangeServiceDelegate)
    {
        _shoppingListExchangeServiceDelegate = shoppingListExchangeServiceDelegate;
    }

    public async Task HandleAsync(ItemUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var service = _shoppingListExchangeServiceDelegate(cancellationToken);
        await service.ExchangeItemAsync(domainEvent.OldItemId, domainEvent.NewItem, cancellationToken);
    }
}