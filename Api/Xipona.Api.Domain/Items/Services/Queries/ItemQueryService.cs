using Xipona.Api.Domain.Common.Exceptions;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Ports;
using Xipona.Api.Domain.Items.Reasons;
using Xipona.Api.Domain.Items.Services.Conversion.ItemReadModels;
using Xipona.Api.Domain.ShoppingLists.Reasons;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Items.Services.Queries;

public class ItemQueryService : IItemQueryService
{
    private readonly IItemRepository _itemRepository;
    private readonly IItemReadModelConversionService _itemReadModelConversionService;

    public ItemQueryService(IItemRepository itemRepository,
        IItemReadModelConversionService itemReadModelConversionService)
    {
        _itemRepository = itemRepository;
        _itemReadModelConversionService = itemReadModelConversionService;
    }

    public async Task<ItemReadModel> GetAsync(ItemId itemId)
    {
        var item = await _itemRepository.FindActiveByAsync(itemId);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(itemId));

        return await _itemReadModelConversionService.ConvertAsync(item);
    }

    public async Task<ItemTypePricesReadModel> GetItemTypePrices(ItemId itemId, StoreId storeId)
    {
        var item = await _itemRepository.FindActiveByAsync(itemId);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(itemId));

        if (!item.HasItemTypes)
            throw new DomainException(new ItemHasNoItemTypesReason(itemId));

        var itemTypePrices = item.ItemTypes
            .Where(t => t.IsAvailableAt(storeId))
            .Select(t =>
            {
                var av = t.Availabilities.First(av => av.StoreId == storeId);
                return new ItemTypePriceReadModel(t.Id, av.Price, t.Name);
            })
            .ToList();

        if (itemTypePrices.Count == 0)
            throw new DomainException(new ItemAtStoreNotAvailableReason(itemId, storeId));

        return new ItemTypePricesReadModel(item.Id, storeId, itemTypePrices);
    }
}