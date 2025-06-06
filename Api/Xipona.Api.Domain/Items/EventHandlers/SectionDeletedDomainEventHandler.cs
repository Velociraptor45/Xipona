using Microsoft.Extensions.Logging;
using Xipona.Api.Core.DomainEventHandlers;
using Xipona.Api.Domain.Items.Services.Modifications;
using Xipona.Api.Domain.Stores.DomainEvents;

namespace Xipona.Api.Domain.Items.EventHandlers;

public class SectionDeletedDomainEventHandler : IDomainEventHandler<SectionDeletedDomainEvent>
{
    private readonly Func<CancellationToken, IItemModificationService> _itemModificationServiceDelegate;
    private readonly ILogger<SectionDeletedDomainEventHandler> _logger;

    public SectionDeletedDomainEventHandler(Func<CancellationToken, IItemModificationService> itemModificationServiceDelegate,
        ILogger<SectionDeletedDomainEventHandler> logger)
    {
        _itemModificationServiceDelegate = itemModificationServiceDelegate;
        _logger = logger;
    }

    public async Task HandleAsync(SectionDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Started handling {EventName} for items", nameof(SectionDeletedDomainEvent));

        var service = _itemModificationServiceDelegate(cancellationToken);
        await service.TransferToSectionAsync(domainEvent.SectionId);

        _logger.LogDebug("Finished handling {EventName} for items", nameof(SectionDeletedDomainEvent));
    }
}
