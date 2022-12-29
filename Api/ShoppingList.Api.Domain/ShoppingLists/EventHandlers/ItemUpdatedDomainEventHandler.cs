using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Exchanges;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.EventHandlers;

public class ItemUpdatedDomainEventHandler : IDomainEventHandler<ItemUpdatedDomainEvent>
{
    private readonly IShoppingListExchangeService _shoppingListExchangeService;

    public ItemUpdatedDomainEventHandler(IShoppingListExchangeService shoppingListExchangeService)
    {
        _shoppingListExchangeService = shoppingListExchangeService;
    }

    public async Task HandleAsync(ItemUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await _shoppingListExchangeService.ExchangeItemAsync(domainEvent.OldItemId, domainEvent.NewItem,
            cancellationToken);
    }
}