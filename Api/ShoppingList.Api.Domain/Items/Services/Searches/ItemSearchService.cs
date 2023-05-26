using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Conversion;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Conversion.ItemSearchReadModels;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;

public class ItemSearchService : IItemSearchService
{
    private const int _maxSearchResults = 20;

    private readonly IItemRepository _itemRepository;
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IItemTypeReadRepository _itemTypeReadRepository;
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly IItemSearchReadModelConversionService _itemSearchReadModelConversionService;
    private readonly IValidator _validator;
    private readonly IItemAvailabilityReadModelConversionService _availabilityConverter;
    private readonly CancellationToken _cancellationToken;

    public ItemSearchService(
        IItemRepository itemRepository,
        Func<CancellationToken, IShoppingListRepository> shoppingListRepositoryDelegate,
        Func<CancellationToken, IStoreRepository> storeRepositoryDelegate,
        Func<CancellationToken, IItemTypeReadRepository> itemTypeReadRepositoryDelegate,
        Func<CancellationToken, IItemCategoryRepository> itemCategoryRepositoryDelegate,
        Func<CancellationToken, IItemSearchReadModelConversionService> itemSearchReadModelConversionServiceDelegate,
        Func<CancellationToken, IValidator> validatorDelegate,
        Func<CancellationToken, IItemAvailabilityReadModelConversionService> availabilityConverterDelegate,
        CancellationToken cancellationToken)
    {
        _itemRepository = itemRepository;
        _shoppingListRepository = shoppingListRepositoryDelegate(cancellationToken);
        _storeRepository = storeRepositoryDelegate(cancellationToken);
        _itemTypeReadRepository = itemTypeReadRepositoryDelegate(cancellationToken);
        _itemCategoryRepository = itemCategoryRepositoryDelegate(cancellationToken);
        _itemSearchReadModelConversionService = itemSearchReadModelConversionServiceDelegate(cancellationToken);
        _validator = validatorDelegate(cancellationToken);
        _availabilityConverter = availabilityConverterDelegate(cancellationToken);
        _cancellationToken = cancellationToken;
    }

    public async Task<IEnumerable<SearchItemResultReadModel>> SearchAsync(IEnumerable<StoreId> storeIds,
        IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds)
    {
        var items = await _itemRepository.FindPermanentByAsync(storeIds, itemCategoriesIds,
            manufacturerIds, _cancellationToken);

        return items
            .Where(model => !model.IsDeleted)
            .Select(model => new SearchItemResultReadModel(model));
    }

    public async Task<IEnumerable<SearchItemResultReadModel>> SearchAsync(string searchInput)
    {
        if (string.IsNullOrWhiteSpace(searchInput))
            return Enumerable.Empty<SearchItemResultReadModel>();

        var items = await _itemRepository.FindActiveByAsync(searchInput, _cancellationToken);
        return items.Select(i => new SearchItemResultReadModel(i.Id, i.Name));
    }

    public async Task<IEnumerable<SearchItemByItemCategoryResult>> SearchAsync(ItemCategoryId itemCategoryId)
    {
        await _validator.ValidateAsync(itemCategoryId);

        var items = (await _itemRepository.FindActiveByAsync(itemCategoryId, _cancellationToken))
            .ToList();
        var itemsLookup = items.ToLookup(i => i.HasItemTypes);

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
                    availabilitiesDict[(item.Id, type.Id)]));
            }
        }

        foreach (var item in itemsLookup[false])
        {
            results.Add(new SearchItemByItemCategoryResult(
                item.Id,
                null,
                item.Name,
                availabilitiesDict[(item.Id, null)]));
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

        var items = (await _itemRepository.FindActiveByAsync(nameTrimmed, storeId, listItemIds.ItemIds, _maxSearchResults,
            _cancellationToken)).ToList();
        var itemCategoryResultLimit = _maxSearchResults - GetResultCount(items, storeId);
        var itemsWithItemCategory = (await LoadItemsForCategory(nameTrimmed, storeId, itemCategoryResultLimit)).ToList();
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

        var itemsWithTypesReadModels = (await _itemSearchReadModelConversionService.ConvertAsync(
            itemsWithTypeNotOnShoppingList, store)).ToList();

        if (itemReadModels.Count + itemsWithTypesReadModels.Count >= _maxSearchResults)
            return itemReadModels.Union(itemsWithTypesReadModels).Take(_maxSearchResults);

        // types
        var typesResultLimit = _maxSearchResults - itemReadModels.Count - itemsWithTypesReadModels.Count;
        var itemsWithMatchingItemTypes = await GetItemsWithMatchingItemTypeIdsAsync(nameTrimmed, storeId,
            searchResultItemsWithTypes.Select(i => i.Id),
            listItemIds.ItemTypeIds.Select(m => m.TypeId),
            typesResultLimit);
        itemsWithTypeNotOnShoppingList.AddRange(itemsWithMatchingItemTypes);

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
                if (!itemTypeIds.Any())
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

            if (!typeIdsNotOnList.Any())
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
        if (!itemTypeIdMappings.Any())
            return Enumerable.Empty<ItemWithMatchingItemTypeIds>();

        var itemTypeIdGroups = itemTypeIdMappings
            .GroupBy(mapping => mapping.Item1, mapping => mapping.Item2)
            .ToList();

        var itemIds = itemTypeIdGroups.Select(group => group.Key);
        var itemsDict = (await _itemRepository.FindActiveByAsync(itemIds, _cancellationToken))
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

    private async Task<IEnumerable<IItem>> LoadItemsForCategory(string name, StoreId storeId, int limit)
    {
        if (limit <= 0)
            return Enumerable.Empty<IItem>();

        var categoryIds = (await _itemCategoryRepository.FindByAsync(name, false, limit))
            .Select(c => c.Id)
            .ToList();

        return categoryIds.Any()
            ? (await _itemRepository.FindActiveByAsync(categoryIds, storeId, _cancellationToken)).ToList()
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