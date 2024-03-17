using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.Items.Reasons;
using ProjectHermes.Xipona.Api.Domain.Shared.Validations;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.TemporaryItems;

public class TemporaryItemService : ITemporaryItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly IValidator _validator;

    public TemporaryItemService(IItemRepository itemRepository, IValidator validator)
    {
        _itemRepository = itemRepository;
        _validator = validator;
    }

    public async Task MakePermanentAsync(PermanentItem permanentItem)
    {
        IItem? item = await _itemRepository.FindActiveByAsync(permanentItem.Id);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(permanentItem.Id));
        if (!item.IsTemporary)
            throw new DomainException(new ItemNotTemporaryReason(permanentItem.Id));

        var itemCategoryId = permanentItem.ItemCategoryId;
        var manufacturerId = permanentItem.ManufacturerId;

        await _validator.ValidateAsync(itemCategoryId);

        if (manufacturerId != null)
        {
            await _validator.ValidateAsync(manufacturerId.Value);
        }

        var availabilities = permanentItem.Availabilities;
        await _validator.ValidateAsync(availabilities);

        item.MakePermanent(permanentItem, availabilities);
        await _itemRepository.StoreAsync(item);
    }
}