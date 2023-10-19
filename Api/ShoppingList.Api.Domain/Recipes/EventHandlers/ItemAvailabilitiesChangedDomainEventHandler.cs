using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.EventHandlers;

public class ItemAvailabilitiesChangedDomainEventHandler : IDomainEventHandler<ItemAvailabilitiesChangedDomainEvent>
{
    private readonly Func<CancellationToken, IRecipeModificationService> _recipeModificationServiceDelegate;
    private readonly ILogger<ItemAvailabilityDeletedDomainEventHandler> _logger;

    public ItemAvailabilitiesChangedDomainEventHandler(
        Func<CancellationToken, IRecipeModificationService> recipeModificationServiceDelegate,
        ILogger<ItemAvailabilityDeletedDomainEventHandler> logger)
    {
        _recipeModificationServiceDelegate = recipeModificationServiceDelegate;
        _logger = logger;
    }

    public async Task HandleAsync(ItemAvailabilitiesChangedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogDebug(
            () => $"Started handling {nameof(ItemAvailabilitiesChangedDomainEvent)} for item '{domainEvent.ItemId}' for recipes");

        var service = _recipeModificationServiceDelegate(cancellationToken);
        await service.ModifyIngredientsAfterAvailabilitiesChangedAsync(domainEvent.ItemId, domainEvent.ItemTypeId,
            domainEvent.NewAvailabilities);

        _logger.LogDebug(
            () => $"Finished handling {nameof(ItemAvailabilitiesChangedDomainEvent)} for item '{domainEvent.ItemId}' for recipes");
    }
}