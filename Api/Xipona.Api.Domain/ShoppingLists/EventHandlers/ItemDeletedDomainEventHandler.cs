using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
using ProjectHermes.Xipona.Api.Domain.Items.DomainEvents;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Modifications;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.EventHandlers;

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