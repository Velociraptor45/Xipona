using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.TemporaryItems;

public class TemporaryItemService : ITemporaryItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly IValidator _validator;
    private readonly CancellationToken _cancellationToken;

    public TemporaryItemService(IItemRepository itemRepository,
        Func<CancellationToken, IValidator> validatorDelegate,
        CancellationToken cancellationToken)
    {
        _itemRepository = itemRepository;
        _validator = validatorDelegate(cancellationToken);
        _cancellationToken = cancellationToken;
    }

    public async Task MakePermanentAsync(PermanentItem permanentItem)
    {
        IItem? item = await _itemRepository.FindActiveByAsync(permanentItem.Id, _cancellationToken);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(permanentItem.Id));
        if (!item.IsTemporary)
            throw new DomainException(new ItemNotTemporaryReason(permanentItem.Id));

        var itemCategoryId = permanentItem.ItemCategoryId;
        var manufacturerId = permanentItem.ManufacturerId;

        await _validator.ValidateAsync(itemCategoryId);

        _cancellationToken.ThrowIfCancellationRequested();

        if (manufacturerId != null)
        {
            await _validator.ValidateAsync(manufacturerId.Value);
        }

        var availabilities = permanentItem.Availabilities;
        await _validator.ValidateAsync(availabilities);

        _cancellationToken.ThrowIfCancellationRequested();

        item.MakePermanent(permanentItem, availabilities);
        await _itemRepository.StoreAsync(item, _cancellationToken);
    }
}