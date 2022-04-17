using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.TemporaryItems;

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
        ArgumentNullException.ThrowIfNull(permanentItem);

        IStoreItem? storeItem = await _itemRepository.FindByAsync(permanentItem.Id, _cancellationToken);
        if (storeItem == null)
            throw new DomainException(new ItemNotFoundReason(permanentItem.Id));
        if (!storeItem.IsTemporary)
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

        storeItem.MakePermanent(permanentItem, availabilities);
        await _itemRepository.StoreAsync(storeItem, _cancellationToken);
    }
}