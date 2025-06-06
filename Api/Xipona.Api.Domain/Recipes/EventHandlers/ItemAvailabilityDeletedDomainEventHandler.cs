using Microsoft.Extensions.Logging;
using Xipona.Api.Core.DomainEventHandlers;
using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.Items.DomainEvents;
using Xipona.Api.Domain.Recipes.Services.Modifications;

namespace Xipona.Api.Domain.Recipes.EventHandlers;

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
            "Started handling {EventName} for item '{ItemId}' for recipes",
            nameof(ItemAvailabilityDeletedDomainEvent),
            domainEvent.ItemId);

        var service = _recipeModificationServiceDelegate(cancellationToken);
        await service.ModifyIngredientsAfterAvailabilityWasDeletedAsync(domainEvent.ItemId, null,
            domainEvent.Availability.StoreId);

        _logger.LogDebug(
            "Finished handling {EventName} for item '{ItemId}' for recipes",
            nameof(ItemAvailabilityDeletedDomainEvent),
            domainEvent.ItemId);
    }
}