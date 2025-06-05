using Microsoft.Extensions.Caching.Memory;
using Xipona.Api.Domain.Common.Exceptions;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.ItemCategories.Ports;
using Xipona.Api.Domain.ItemCategories.Reasons;
using Xipona.Api.Domain.ItemCategories.Services.Shared;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Queries;
using Xipona.Api.Domain.Items.Services.Queries.Quantities;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.Manufacturers.Ports;
using Xipona.Api.Domain.Manufacturers.Reasons;
using Xipona.Api.Domain.Manufacturers.Services.Shared;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.Stores.Ports;

namespace Xipona.Api.Domain.Items.Services.Conversion.ItemReadModels;

public class ItemReadModelConversionService : IItemReadModelConversionService
{
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IMemoryCache _cache;

    public ItemReadModelConversionService(IItemCategoryRepository itemCategoryRepository,
        IManufacturerRepository manufacturerRepository, IStoreRepository storeRepository,
        IMemoryCache cache)
    {
        _itemCategoryRepository = itemCategoryRepository;
        _manufacturerRepository = manufacturerRepository;
        _storeRepository = storeRepository;
        _cache = cache;
    }

    public async Task<ItemReadModel> ConvertAsync(IItem item)
    {
        IItemCategory? itemCategory = null;
        IManufacturer? manufacturer = null;

        if (item.ItemCategoryId != null)
        {
            itemCategory = await _itemCategoryRepository.FindByAsync(item.ItemCategoryId.Value);
            if (itemCategory == null)
                throw new DomainException(new ItemCategoryNotFoundReason(item.ItemCategoryId.Value));
        }
        if (item.ManufacturerId != null)
        {
            manufacturer = await _manufacturerRepository.FindByAsync(item.ManufacturerId.Value);
            if (manufacturer == null)
                throw new DomainException(new ManufacturerNotFoundReason(item.ManufacturerId.Value));
        }

        var storeIds = item.Availabilities.Select(av => av.StoreId).ToList();
        storeIds.AddRange(item.ItemTypes.SelectMany(t => t.Availabilities.Select(av => av.StoreId)));
        storeIds = storeIds.Distinct().ToList();
        var storeDict = (await _storeRepository.FindActiveByAsync(storeIds))
            .ToDictionary(store => store.Id);

        return ToReadModel(item, itemCategory, manufacturer, storeDict);
    }

    private ItemReadModel ToReadModel(IItem model, IItemCategory? itemCategory,
        IManufacturer? manufacturer, IReadOnlyDictionary<StoreId, IStore> stores)
    {
        var availabilityReadModels = ToAvailabilityReadModel(model.Availabilities, stores).ToList();

        var itemTypeReadModels = new List<ItemTypeReadModel>();
        foreach (var itemType in model.ItemTypes)
        {
            if (itemType.IsDeleted)
                continue;

            var itemTypeReadModel = new ItemTypeReadModel(itemType.Id, itemType.Name,
                ToAvailabilityReadModel(itemType.Availabilities, stores));
            itemTypeReadModels.Add(itemTypeReadModel);
        }

        var itemQuantityInPacket = model.ItemQuantity.InPacket;
        var quantityTypeInPacketReadModel = itemQuantityInPacket is null
            ? null
            : new QuantityTypeInPacketReadModel(itemQuantityInPacket.Type);

        return new ItemReadModel(
            model.Id,
            model.Name,
            model.IsDeleted,
            model.Comment,
            model.IsTemporary,
            new QuantityTypeReadModel(model.ItemQuantity.Type, _cache),
            itemQuantityInPacket?.Quantity,
            quantityTypeInPacketReadModel,
            itemCategory is null ?
                null :
                new ItemCategoryReadModel(itemCategory),
            manufacturer is null ?
                null :
                new ManufacturerReadModel(manufacturer),
            availabilityReadModels,
            itemTypeReadModels);
    }

    private static IEnumerable<ItemAvailabilityReadModel> ToAvailabilityReadModel(
        IEnumerable<ItemAvailability> availabilities, IReadOnlyDictionary<StoreId, IStore> stores)
    {
        foreach (var av in availabilities)
        {
            var store = stores[av.StoreId];
            var section = store.Sections.First(s => s.Id == av.DefaultSectionId);

            yield return new ItemAvailabilityReadModel(av, store, section);
        }
    }
}