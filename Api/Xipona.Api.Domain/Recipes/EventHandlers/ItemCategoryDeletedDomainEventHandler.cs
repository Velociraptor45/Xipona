using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.DomainEvents;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Modifications;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.EventHandlers;

public class ItemCategoryDeletedDomainEventHandler : IDomainEventHandler<ItemCategoryDeletedDomainEvent>
{
    private readonly Func<CancellationToken, IRecipeModificationService> _recipeModificationServiceDelegate;

    public ItemCategoryDeletedDomainEventHandler(
        Func<CancellationToken, IRecipeModificationService> recipeModificationServiceDelegate)
    {
        _recipeModificationServiceDelegate = recipeModificationServiceDelegate;
    }

    public async Task HandleAsync(ItemCategoryDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var service = _recipeModificationServiceDelegate(cancellationToken);
        await service.RemoveIngredientsOfItemCategoryAsync(domainEvent.ItemCategoryId);
    }
}