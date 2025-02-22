using Microsoft.Extensions.Logging;
using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Items.DomainEvents;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Modifications;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.EventHandlers;

public class ItemTypeDeletedDomainEventHandler : IDomainEventHandler<ItemTypeDeletedDomainEvent>
{
    private readonly Func<CancellationToken, IRecipeModificationService> _recipeModificationServiceDelegate;
    private readonly ILogger<ItemTypeDeletedDomainEventHandler> _logger;

    public ItemTypeDeletedDomainEventHandler(
        Func<CancellationToken, IRecipeModificationService> recipeModificationServiceDelegate,
        ILogger<ItemTypeDeletedDomainEventHandler> logger)
    {
        _recipeModificationServiceDelegate = recipeModificationServiceDelegate;
        _logger = logger;
    }

    public async Task HandleAsync(ItemTypeDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogDebug(
            "Started handling {EventName} for item '{ItemId}' and type '{ItemTypeId}' for recipes",
            nameof(ItemTypeDeletedDomainEvent),
            domainEvent.ItemId.Value,
            domainEvent.ItemTypeId.Value);

        var service = _recipeModificationServiceDelegate(cancellationToken);
        await service.RemoveDefaultItemAsync(domainEvent.ItemId, domainEvent.ItemTypeId);

        _logger.LogDebug(
            "Finished handling {EventName} for item '{ItemId}' and type '{ItemTypeId}' for recipes",
            nameof(ItemTypeDeletedDomainEvent),
            domainEvent.ItemId.Value,
            domainEvent.ItemTypeId.Value);
    }
}