using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
using ProjectHermes.Xipona.Api.Domain.Items.DomainEvents;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Exchanges;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.EventHandlers;

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
        await service.ExchangeItemAsync(domainEvent.ItemId, domainEvent.NewItem);
    }
}