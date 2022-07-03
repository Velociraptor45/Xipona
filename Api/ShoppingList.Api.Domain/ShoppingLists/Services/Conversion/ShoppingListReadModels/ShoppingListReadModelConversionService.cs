using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries.Quantities;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Conversion.ShoppingListReadModels;

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

    public async Task<ShoppingListReadModel> ConvertAsync(IShoppingList shoppingList, CancellationToken cancellationToken)
    {
        var itemIds = shoppingList.Items.Select(i => i.Id);
        var itemsDict = (await _itemRepository.FindByAsync(itemIds, cancellationToken))
            .ToDictionary(i => i.Id);

        var itemCategoryIds = itemsDict.Values.Where(i => i.ItemCategoryId != null)
            .Select(i => i.ItemCategoryId!.Value);
        var itemCategoriesDict = (await _itemCategoryRepository.FindByAsync(itemCategoryIds, cancellationToken))
            .ToDictionary(cat => cat.Id);

        var manufacturerIds = itemsDict.Values.Where(i => i.ManufacturerId != null)
            .Select(i => i.ManufacturerId!.Value);
        var manufacturersDict = (await _manufacturerRepository.FindByAsync(manufacturerIds, cancellationToken))
            .ToDictionary(man => man.Id);

        IStore? store = await _storeRepository.FindByAsync(shoppingList.StoreId, cancellationToken);
        if (store is null)
            throw new DomainException(new StoreNotFoundReason(shoppingList.StoreId));

        return ToReadModel(shoppingList, store, itemsDict, itemCategoriesDict, manufacturersDict);
    }

    private static ShoppingListReadModel ToReadModel(IShoppingList shoppingList, IStore store,
        IReadOnlyDictionary<ItemId, IItem> storeItems, IReadOnlyDictionary<ItemCategoryId,
            IItemCategory> itemCategories, IReadOnlyDictionary<ManufacturerId, IManufacturer> manufacturers)
    {
        List<ShoppingListSectionReadModel> sectionReadModels = new();
        foreach (var section in shoppingList.Sections)
        {
            List<ShoppingListItemReadModel> itemReadModels = new();
            foreach (var item in section.Items)
            {
                var storeItem = storeItems[item.Id];

                Price price;
                string name;
                if (storeItem.HasItemTypes)
                {
                    if (item.TypeId == null)
                        throw new DomainException(new ShoppingListItemMissingTypeReason(item.Id));

                    var itemType = storeItem.ItemTypes.FirstOrDefault(t => t.Id == item.TypeId);
                    if (itemType == null)
                        throw new DomainException(new ItemTypeNotFoundReason(item.Id, item.TypeId.Value));

                    price = itemType.Availabilities.First(av => av.StoreId == store.Id).Price;
                    name = $"{storeItem.Name} {itemType.Name}";
                }
                else
                {
                    price = storeItem.Availabilities.First(av => av.StoreId == store.Id).Price;
                    name = storeItem.Name.Value;
                }

                var itemQuantityInPacket = storeItem.ItemQuantity.InPacket;
                var quantityTypeInPacketReadModel = itemQuantityInPacket is null
                    ? null
                    : new QuantityTypeInPacketReadModel(itemQuantityInPacket.Type);

                var itemReadModel = new ShoppingListItemReadModel(
                    item.Id,
                    item.TypeId,
                    name,
                    storeItem.IsDeleted,
                    storeItem.Comment,
                    storeItem.IsTemporary,
                    price,
                    new QuantityTypeReadModel(storeItem.ItemQuantity.Type),
                    itemQuantityInPacket?.Quantity,
                    quantityTypeInPacketReadModel,
                    storeItem.ItemCategoryId == null ?
                        null : new ItemCategoryReadModel(itemCategories[storeItem.ItemCategoryId.Value]),
                    storeItem.ManufacturerId == null ?
                        null : new ManufacturerReadModel(manufacturers[storeItem.ManufacturerId.Value]),
                    item.IsInBasket,
                    item.Quantity);

                itemReadModels.Add(itemReadModel);
            }

            var storeSection = store.Sections.First(s => s.Id == section.Id);

            var sectionReadModel = new ShoppingListSectionReadModel(
                section.Id,
                storeSection.Name,
                storeSection.SortingIndex,
                storeSection.IsDefaultSection,
                itemReadModels);

            sectionReadModels.Add(sectionReadModel);
        }

        return new ShoppingListReadModel(
            shoppingList.Id,
            shoppingList.CompletionDate,
            new ShoppingListStoreReadModel(store.Id, store.Name),
            sectionReadModels);
    }
}