using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.EventHandlers;

public class ItemDeletedDomainEventHandler : IDomainEventHandler<ItemDeletedDomainEvent>
{
    public Task HandleAsync(ItemDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}