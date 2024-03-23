using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Ports;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.Items.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Conversion;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Conversion.ItemSearchReadModels;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Ports;
using ProjectHermes.Xipona.Api.Domain.Shared.Validations;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Ports;
using ProjectHermes.Xipona.Api.Domain.Stores.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;

public class ItemSearchService : IItemSearchService
{
    private const int _maxSearchResults = 20;

    private readonly IItemRepository _itemRepository;
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IItemTypeReadRepository _itemTypeReadRepository;
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly IItemSearchReadModelConversionService _itemSearchReadModelConversionService;
    private readonly IValidator _validator;
    private readonly IItemAvailabilityReadModelConversionService _availabilityConverter;

    public ItemSearchService(
        IItemRepository itemRepository,
        IManufacturerRepository manufacturerRepository,
        IShoppingListRepository shoppingListRepository,
        IStoreRepository storeRepository,
        IItemTypeReadRepository itemTypeReadRepository,
        IItemCategoryRepository itemCategoryRepository,
        IItemSearchReadModelConversionService itemSearchReadModelConversionService,
        IValidator validator,
        IItemAvailabilityReadModelConversionService availabilityConverter)
    {
        _itemRepository = itemRepository;
        _manufacturerRepository = manufacturerRepository;
        _shoppingListRepository = shoppingListRepository;
        _storeRepository = storeRepository;
        _itemTypeReadRepository = itemTypeReadRepository;
        _itemCategoryRepository = itemCategoryRepository;
        _itemSearchReadModelConversionService = itemSearchReadModelConversionService;
        _validator = validator;
        _availabilityConverter = availabilityConverter;
    }

    public async Task<IEnumerable<SearchItemResultReadModel>> SearchAsync(IEnumerable<StoreId> storeIds,
        IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds)
    {
        var items = await _itemRepository.FindPermanentByAsync(storeIds, itemCategoriesIds,
            manufacturerIds);

        return items
            .Where(model => !model.IsDeleted)
            .Select(model => new SearchItemResultReadModel(model.Id, model.Name, null));
    }

    public async Task<IEnumerable<SearchItemResultReadModel>> SearchAsync(string searchInput)
    {
        if (string.IsNullOrWhiteSpace(searchInput))
            return Enumerable.Empty<SearchItemResultReadModel>();

        var items = (await _itemRepository.FindActiveByAsync(searchInput)).ToList();

        var manufacturerIds = items.Where(i => i.ManufacturerId is not null).Select(i => i.ManufacturerId!.Value);
        var manufacturers = (await _manufacturerRepository.FindByAsync(manufacturerIds)).ToDictionary(m => m.Id);

        return items.Select(i =>
        {
            var manufacturerName = i.ManufacturerId is null ? null : manufacturers[i.ManufacturerId!.Value].Name;
            return new SearchItemResultReadModel(i.Id, i.Name, manufacturerName);
        });
    }

    public async Task<IEnumerable<SearchItemByItemCategoryResult>> SearchAsync(ItemCategoryId itemCategoryId)
    {
        await _validator.ValidateAsync(itemCategoryId);

        var items = (await _itemRepository.FindActiveByAsync(itemCategoryId))
            .ToList();
        var itemsLookup = items.ToLookup(i => i.HasItemTypes);

        var manufacturerIds = items.Where(i => i.ManufacturerId is not null).Select(i => i.ManufacturerId!.Value);
        var manufacturers = (await _manufacturerRepository.FindByAsync(manufacturerIds)).ToDictionary(m => m.Id);

        var availabilitiesDict = await _availabilityConverter.ConvertAsync(items);

        var results = new List<SearchItemByItemCategoryResult>();
        foreach (var item in itemsLookup[true])
        {
            foreach (var type in item.ItemTypes)
            {
                if (type.IsDeleted)
                    continue;

                results.Add(new SearchItemByItemCategoryResult(
                    item.Id,
                    type.Id,
                    $"{item.Name} {type.Name}",
                    item.ManufacturerId is null ? null : manufacturers[item.ManufacturerId!.Value].Name,
                    availabilitiesDict[(item.Id, type.Id)].ToList()));
            }
        }

        foreach (var item in itemsLookup[false])
        {
            results.Add(new SearchItemByItemCategoryResult(
                item.Id,
                null,
                item.Name,
                item.ManufacturerId is null ? null : manufacturers[item.ManufacturerId!.Value].Name,
                availabilitiesDict[(item.Id, null)].ToList()));
        }

        return results;
    }

    public async Task<IEnumerable<SearchItemForShoppingResultReadModel>> SearchForShoppingListAsync(string name, StoreId storeId)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Enumerable.Empty<SearchItemForShoppingResultReadModel>();

        var store = await LoadStoreAsync(storeId);
        var nameTrimmed = name.Trim();

        var listItemIds = await LoadItemIdsOnShoppingList(storeId);

        var items = (await _itemRepository.FindActiveByAsync(nameTrimmed, storeId, listItemIds.ItemIds, _maxSearchResults)).ToList();
        var itemCategoryResultLimit = _maxSearchResults - GetResultCount(items, storeId);
        var itemsWithItemCategory = (await LoadItemsForCategory(nameTrimmed, storeId, listItemIds.ItemIds, itemCategoryResultLimit)).ToList();
        var searchResultItemGroups = items
            .Union(itemsWithItemCategory)
            .DistinctBy(item => item.Id)
            .ToLookup(i => i.HasItemTypes);

        // items without types
        var searchResultItems = searchResultItemGroups[false];
        var itemReadModels = (await _itemSearchReadModelConversionService.ConvertAsync(
            searchResultItems, store))
            .ToList();

        if (itemReadModels.Count >= _maxSearchResults)
            return itemReadModels.Take(_maxSearchResults);

        // items with types
        var searchResultItemsWithTypes = searchResultItemGroups[true].ToList();
        var itemsWithTypeNotOnShoppingList = GetMatchingItemsWithTypeIds(storeId,
                searchResultItemsWithTypes, listItemIds.ItemTypeIds)
            .ToList();

        // types
        var typesResultLimit = _maxSearchResults - itemReadModels.Count - itemsWithTypeNotOnShoppingList.Count;
        if (typesResultLimit > 0)
        {
            var itemsWithMatchingItemTypes = await GetItemsWithMatchingItemTypeIdsAsync(nameTrimmed, storeId,
                searchResultItemsWithTypes.Select(i => i.Id),
                listItemIds.ItemTypeIds.Select(m => m.TypeId),
                typesResultLimit);
            itemsWithTypeNotOnShoppingList.AddRange(itemsWithMatchingItemTypes);
        }

        var itemsWithTypesReadModels = (await _itemSearchReadModelConversionService.ConvertAsync(
            itemsWithTypeNotOnShoppingList, store)).ToList();

        return itemsWithTypesReadModels.Union(itemReadModels).Take(_maxSearchResults);
    }

    private static IEnumerable<ItemWithMatchingItemTypeIds> GetMatchingItemsWithTypeIds(StoreId storeId,
        IEnumerable<IItem> searchResultItemsWithTypes, IEnumerable<(ItemId, ItemTypeId)> itemIdsOnShoppingList)
    {
        var itemsWithTypesOnShoppingList = itemIdsOnShoppingList.ToLookup(g => g.Item1, g => g.Item2);
        foreach (var item in searchResultItemsWithTypes)
        {
            if (!itemsWithTypesOnShoppingList.Contains(item.Id))
            {
                var itemTypeIds = item
                    .GetTypesFor(storeId)
                    .Where(t => !t.IsDeleted)
                    .Select(t => t.Id)
                    .ToList();
                if (itemTypeIds.Count == 0)
                    continue;

                yield return new ItemWithMatchingItemTypeIds(item, itemTypeIds);
                continue;
            }

            var typeIdsOnList = itemsWithTypesOnShoppingList[item.Id].ToList();
            var typeIdsNotOnList = item
                .GetTypesFor(storeId)
                .Select(t => t.Id)
                .Except(typeIdsOnList)
                .ToList();

            if (typeIdsNotOnList.Count == 0)
                continue;

            yield return new ItemWithMatchingItemTypeIds(item, typeIdsNotOnList);
        }
    }

    private async Task<IEnumerable<ItemWithMatchingItemTypeIds>> GetItemsWithMatchingItemTypeIdsAsync(
        string name, StoreId storeId, IEnumerable<ItemId> itemsWithTypesAlreadyFound,
        IEnumerable<ItemTypeId> itemTypeIdsOnShoppingList, int limit)
    {
        var itemTypeIdMappings =
            (await _itemTypeReadRepository.FindActiveByAsync(name, storeId, itemsWithTypesAlreadyFound,
                itemTypeIdsOnShoppingList, limit))
            .ToList();
        if (itemTypeIdMappings.Count == 0)
            return Enumerable.Empty<ItemWithMatchingItemTypeIds>();

        var itemTypeIdGroups = itemTypeIdMappings
            .GroupBy(mapping => mapping.Item1, mapping => mapping.Item2)
            .ToList();

        var itemIds = itemTypeIdGroups.Select(group => group.Key);
        var itemsDict = (await _itemRepository.FindActiveByAsync(itemIds))
            .ToDictionary(i => i.Id);

        var result = new List<ItemWithMatchingItemTypeIds>();
        foreach (var itemTypeIdGroup in itemTypeIdGroups)
        {
            if (!itemsDict.TryGetValue(itemTypeIdGroup.Key, out var item))
                throw new DomainException(new ItemNotFoundReason(itemTypeIdGroup.Key));

            result.Add(new ItemWithMatchingItemTypeIds(item, itemTypeIdGroup));
        }

        return result;
    }

    private async Task<IEnumerable<IItem>> LoadItemsForCategory(string name, StoreId storeId,
        IEnumerable<ItemId> excludedItemIds, int limit)
    {
        if (limit <= 0)
            return Enumerable.Empty<IItem>();

        var categoryIds = (await _itemCategoryRepository.FindByAsync(name, false, limit))
            .Select(c => c.Id)
            .ToList();

        return categoryIds.Count != 0
            ? (await _itemRepository.FindActiveByAsync(categoryIds, storeId, excludedItemIds)).ToList()
            : new List<IItem>();
    }

    private async Task<ShoppingListItemIds> LoadItemIdsOnShoppingList(StoreId storeId)
    {
        IShoppingList shoppingList = await LoadShoppingListAsync(storeId);
        var itemIdsOnShoppingListGroups = shoppingList.Items
            .Select(item => (item.Id, item.TypeId))
            .ToLookup(tuple => tuple.TypeId == null);
        var itemIdsOnShoppingList = itemIdsOnShoppingListGroups[true].Select(tuple => tuple.Id).ToList();
        var itemIdsWithTypeIdOnShoppingList = itemIdsOnShoppingListGroups[false]
            .Select(t => (t.Id, TypeId: t.TypeId!.Value))
            .ToList();

        return new ShoppingListItemIds(itemIdsOnShoppingList, itemIdsWithTypeIdOnShoppingList);
    }

    private async Task<IShoppingList> LoadShoppingListAsync(StoreId storeId)
    {
        IShoppingList? shoppingList = await _shoppingListRepository
            .FindActiveByAsync(storeId);
        if (shoppingList is null)
            throw new DomainException(new ShoppingListNotFoundReason(storeId));

        return shoppingList;
    }

    private async Task<IStore> LoadStoreAsync(StoreId storeId)
    {
        var store = await _storeRepository.FindActiveByAsync(storeId);
        if (store == null)
            throw new DomainException(new StoreNotFoundReason(storeId));

        return store;
    }

    private int GetResultCount(IList<IItem> items, StoreId storeId)
    {
        return items.Sum(i => i.HasItemTypes ? i.GetTypesFor(storeId).Count : 1);
    }

    private sealed record ShoppingListItemIds(IReadOnlyCollection<ItemId> ItemIds,
        IReadOnlyCollection<(ItemId ItemId, ItemTypeId TypeId)> ItemTypeIds);
}