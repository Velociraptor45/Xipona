using Microsoft.Extensions.Logging;
using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.Stores.DomainEvents;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.EventHandlers;

public class SectionDeletedDomainEventHandler : IDomainEventHandler<SectionDeletedDomainEvent>
{
    private readonly Func<CancellationToken, IShoppingListModificationService> _shoppingListModificationServiceDelegate;
    private readonly ILogger<SectionDeletedDomainEventHandler> _logger;

    public SectionDeletedDomainEventHandler(
        Func<CancellationToken, IShoppingListModificationService> shoppingListModificationServiceDelegate,
        ILogger<SectionDeletedDomainEventHandler> logger)
    {
        _shoppingListModificationServiceDelegate = shoppingListModificationServiceDelegate;
        _logger = logger;
    }

    public async Task HandleAsync(SectionDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Started handling {EventName} for items", nameof(SectionDeletedDomainEvent));

        var service = _shoppingListModificationServiceDelegate(cancellationToken);
        await service.RemoveSectionAsync(domainEvent.SectionId);

        _logger.LogDebug("Finished handling {EventName} for items", nameof(SectionDeletedDomainEvent));
    }
}
