using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
using ProjectHermes.Xipona.Api.Domain.Items.DomainEvents;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Modifications;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.EventHandlers;

public class ItemDeletedDomainEventHandler : IDomainEventHandler<ItemDeletedDomainEvent>
{
    private readonly Func<CancellationToken, IRecipeModificationService> _recipeModificationServiceDelegate;

    public ItemDeletedDomainEventHandler(
        Func<CancellationToken, IRecipeModificationService> recipeModificationServiceDelegate)
    {
        _recipeModificationServiceDelegate = recipeModificationServiceDelegate;
    }

    public async Task HandleAsync(ItemDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var service = _recipeModificationServiceDelegate(cancellationToken);
        await service.RemoveDefaultItemAsync(domainEvent.ItemId, null);
    }
}