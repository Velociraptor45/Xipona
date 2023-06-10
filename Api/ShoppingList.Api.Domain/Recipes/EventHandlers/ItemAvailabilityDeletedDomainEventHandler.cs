﻿using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.EventHandlers;

public class ItemAvailabilityDeletedDomainEventHandler : IDomainEventHandler<ItemAvailabilityDeletedDomainEvent>
{
    private readonly Func<CancellationToken, IRecipeModificationService> _recipeModificationServiceDelegate;
    private readonly ILogger<ItemAvailabilityDeletedDomainEventHandler> _logger;

    public ItemAvailabilityDeletedDomainEventHandler(
        Func<CancellationToken, IRecipeModificationService> recipeModificationServiceDelegate,
        ILogger<ItemAvailabilityDeletedDomainEventHandler> logger)
    {
        _recipeModificationServiceDelegate = recipeModificationServiceDelegate;
        _logger = logger;
    }

    public async Task HandleAsync(ItemAvailabilityDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogDebug(
            () => $"Started handling {nameof(ItemAvailabilityDeletedDomainEvent)} for item '{domainEvent.ItemId.Value}' for recipes");

        var service = _recipeModificationServiceDelegate(cancellationToken);
        await service.ModifyIngredientsAfterAvailabilityWasDeletedAsync(domainEvent.ItemId, null,
            domainEvent.Availability.StoreId);

        _logger.LogDebug(
            () => $"Started handling {nameof(ItemAvailabilityDeletedDomainEvent)} for item '{domainEvent.ItemId.Value}' for recipes");
    }
}