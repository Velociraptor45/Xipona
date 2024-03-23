using ProjectHermes.Xipona.Api.Core.Services;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.Items.Reasons;
using ProjectHermes.Xipona.Api.Domain.Shared.Validations;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Updates;

public class ItemUpdateService : IItemUpdateService
{
    private readonly IItemRepository _itemRepository;
    private readonly IDateTimeService _dateTimeService;
    private readonly IValidator _validator;

    public ItemUpdateService(IItemRepository itemRepository, IValidator validator, IDateTimeService dateTimeService)
    {
        _itemRepository = itemRepository;
        _dateTimeService = dateTimeService;
        _validator = validator;
    }

    public async Task UpdateAsync(ItemWithTypesUpdate update)
    {
        var oldItem = await _itemRepository.FindActiveByAsync(update.OldId);
        if (oldItem == null)
            throw new DomainException(new ItemNotFoundReason(update.OldId));

        var updatedItem = await oldItem.UpdateAsync(update, _validator, _dateTimeService);

        await _itemRepository.StoreAsync(updatedItem);
        await _itemRepository.StoreAsync(oldItem);
    }

    public async Task UpdateAsync(ItemUpdate update)
    {
        IItem? oldItem = await _itemRepository.FindActiveByAsync(update.OldId);
        if (oldItem == null)
            throw new DomainException(new ItemNotFoundReason(update.OldId));

        var updatedItem = await oldItem.UpdateAsync(update, _validator, _dateTimeService);

        await _itemRepository.StoreAsync(updatedItem);
        await _itemRepository.StoreAsync(oldItem);
    }

    public async Task UpdateAsync(ItemId itemId, ItemTypeId? itemTypeId, StoreId storeId, Price price)
    {
        IItem? oldItem = await _itemRepository.FindActiveByAsync(itemId);
        if (oldItem == null)
            throw new DomainException(new ItemNotFoundReason(itemId));
        if (oldItem.IsTemporary)
            throw new DomainException(new TemporaryItemNotUpdateableReason(itemId));

        IItem updatedItem = oldItem.Update(storeId, itemTypeId, price, _dateTimeService);

        await _itemRepository.StoreAsync(updatedItem);
        await _itemRepository.StoreAsync(oldItem);
    }
}