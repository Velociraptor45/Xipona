using Xipona.Api.Domain.Common.Exceptions;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.ItemCategories.Ports;
using Xipona.Api.Domain.ItemCategories.Services.Shared;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Ports;
using Xipona.Api.Domain.Items.Reasons;
using Xipona.Api.Domain.Items.Services.Queries.Quantities;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.Manufacturers.Ports;
using Xipona.Api.Domain.Manufacturers.Services.Shared;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Domain.ShoppingLists.Reasons;
using Xipona.Api.Domain.ShoppingLists.Services.Queries;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.Stores.Ports;
using Xipona.Api.Domain.Stores.Reasons;

namespace Xipona.Api.Domain.ShoppingLists.Services.Conversion.ShoppingListReadModels;

public class ShoppingListReadModelConversionService : IShoppingListReadModelConversionService
{
    private readonly IStoreRepository _storeRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly IManufacturerRepository _manufacturerRepository;

    public ShoppingListReadModelConversionService(IStoreRepository storeRepository, IItemRepository itemRepository,
        IItemCategoryRepository itemCategoryRepository, IManufacturerRepository manufacturerRepository)
    {
        _storeRepository = storeRepository;
        _itemRepository = itemRepository;
        _itemCategoryRepository = itemCategoryRepository;
        _manufacturerRepository = manufacturerRepository;
    }

    public async Task<ShoppingListReadModel> ConvertAsync(IShoppingList shoppingList)
    {
        var itemIds = shoppingList.Items.Select(i => i.Id);
        var itemsDict = (await _itemRepository.FindByAsync(itemIds))
            .ToDictionary(i => i.Id);

        var itemCategoryIds = itemsDict.Values.Where(i => i.ItemCategoryId != null)
            .Select(i => i.ItemCategoryId!.Value);
        var itemCategoriesDict = (await _itemCategoryRepository.FindByAsync(itemCategoryIds))
            .ToDictionary(cat => cat.Id);

        var manufacturerIds = itemsDict.Values.Where(i => i.ManufacturerId != null)
            .Select(i => i.ManufacturerId!.Value);
        var manufacturersDict = (await _manufacturerRepository.FindByAsync(manufacturerIds))
            .ToDictionary(man => man.Id);

        IStore? store = await _storeRepository.FindByAsync(shoppingList.StoreId);
        if (store is null)
            throw new DomainException(new StoreNotFoundReason(shoppingList.StoreId));

        return ToReadModel(shoppingList, store, itemsDict, itemCategoriesDict, manufacturersDict);
    }

    private ShoppingListReadModel ToReadModel(IShoppingList shoppingList, IStore store,
        IReadOnlyDictionary<ItemId, IItem> items, IReadOnlyDictionary<ItemCategoryId,
            IItemCategory> itemCategories, IReadOnlyDictionary<ManufacturerId, IManufacturer> manufacturers)
    {
        List<ShoppingListSectionReadModel> sectionReadModels = new();
        foreach (var shoppingListSection in shoppingList.Sections)
        {
            List<ShoppingListItemReadModel> itemReadModels = new();
            foreach (var sectionItem in shoppingListSection.Items)
            {
                var item = items[sectionItem.Id];

                var discount = shoppingList.GetDiscountFor(sectionItem.Id, sectionItem.TypeId);
                Price? price = discount?.Price;

                string name;
                if (item.HasItemTypes)
                {
                    if (sectionItem.TypeId == null)
                        throw new DomainException(new ShoppingListItemMissingTypeReason(sectionItem.Id));

                    var itemType = item.ItemTypes.FirstOrDefault(t => t.Id == sectionItem.TypeId);
                    if (itemType == null)
                        throw new DomainException(new ItemTypeNotFoundReason(sectionItem.Id, sectionItem.TypeId.Value));

                    price ??= itemType.Availabilities.First(av => av.StoreId == store.Id).Price;
                    name = $"{item.Name} {itemType.Name}";
                }
                else
                {
                    price ??= item.Availabilities.First(av => av.StoreId == store.Id).Price;
                    name = item.Name;
                }

                var itemQuantityInPacket = item.ItemQuantity.InPacket;
                var quantityTypeInPacketReadModel = itemQuantityInPacket is null
                    ? null
                    : new QuantityTypeInPacketReadModel(itemQuantityInPacket.Type);

                var itemReadModel = new ShoppingListItemReadModel(
                    sectionItem.Id,
                    sectionItem.TypeId,
                    name,
                    item.IsDeleted,
                    item.Comment,
                    item.IsTemporary,
                    price.Value,
                    new QuantityTypeReadModel(item.ItemQuantity.Type),
                    itemQuantityInPacket?.Quantity,
                    quantityTypeInPacketReadModel,
                    item.ItemCategoryId == null ?
                        null : new ItemCategoryReadModel(itemCategories[item.ItemCategoryId.Value]),
                    item.ManufacturerId == null ?
                        null : new ManufacturerReadModel(manufacturers[item.ManufacturerId.Value]),
                    sectionItem.IsInBasket,
                    sectionItem.Quantity,
                    discount is not null);

                itemReadModels.Add(itemReadModel);
            }

            var section = store.Sections.First(s => s.Id == shoppingListSection.Id);

            var sectionReadModel = new ShoppingListSectionReadModel(
                shoppingListSection.Id,
                section.Name,
                section.SortingIndex,
                section.IsDefaultSection,
                itemReadModels);

            sectionReadModels.Add(sectionReadModel);
        }

        return new ShoppingListReadModel(
            shoppingList.Id,
            shoppingList.CompletionDate,
            new ShoppingListStoreReadModel(store.Id, store.Name),
            sectionReadModels,
            shoppingList.ListDiscounts);
    }
}