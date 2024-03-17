using Microsoft.Extensions.Logging;
using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Items.DomainEvents;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Modifications;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.EventHandlers;

public class ItemTypeAvailabilityDeletedDomainEventHandler : IDomainEventHandler<ItemTypeAvailabilityDeletedDomainEvent>
{
    private readonly Func<CancellationToken, IRecipeModificationService> _recipeModificationServiceDelegate;
    private readonly ILogger<ItemTypeAvailabilityDeletedDomainEventHandler> _logger;

    public ItemTypeAvailabilityDeletedDomainEventHandler(
        Func<CancellationToken, IRecipeModificationService> recipeModificationServiceDelegate,
        ILogger<ItemTypeAvailabilityDeletedDomainEventHandler> logger)
    {
        _recipeModificationServiceDelegate = recipeModificationServiceDelegate;
        _logger = logger;
    }

    public async Task HandleAsync(ItemTypeAvailabilityDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogDebug(
            () => "Started handling {EventName} for item '{ItemId}' and type '{ItemTypeId}' for recipes",
            nameof(ItemTypeAvailabilityDeletedDomainEvent),
            domainEvent.ItemId.Value);

        var service = _recipeModificationServiceDelegate(cancellationToken);
        await service.ModifyIngredientsAfterAvailabilityWasDeletedAsync(domainEvent.ItemId, domainEvent.ItemTypeId,
            domainEvent.Availability.StoreId);

        _logger.LogDebug(
            () => "Finished handling {EventName} for item '{ItemId}' and type '{ItemTypeId}' for recipes",
            nameof(ItemTypeAvailabilityDeletedDomainEvent),
            domainEvent.ItemId.Value,
            domainEvent.ItemTypeId.Value);
    }
}