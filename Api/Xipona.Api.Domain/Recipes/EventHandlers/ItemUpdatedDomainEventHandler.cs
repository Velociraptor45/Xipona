using Xipona.Api.Core.DomainEventHandlers;
using Xipona.Api.Domain.Items.DomainEvents;
using Xipona.Api.Domain.Recipes.Services.Modifications;

namespace Xipona.Api.Domain.Recipes.EventHandlers;

public class ItemUpdatedDomainEventHandler : IDomainEventHandler<ItemUpdatedDomainEvent>
{
    private readonly Func<CancellationToken, IRecipeModificationService> _recipeModificationServiceDelegate;

    public ItemUpdatedDomainEventHandler(
        Func<CancellationToken, IRecipeModificationService> recipeModificationServiceDelegate)
    {
        _recipeModificationServiceDelegate = recipeModificationServiceDelegate;
    }

    public async Task HandleAsync(ItemUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var service = _recipeModificationServiceDelegate(cancellationToken);
        await service.ModifyIngredientsAfterItemUpdateAsync(domainEvent.ItemId, domainEvent.NewItem);
    }
}