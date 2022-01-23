using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.ItemSearchReadModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services
{
    public class ItemQueryService : IItemQueryService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IShoppingListRepository _shoppingListRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IItemTypeReadRepository _itemTypeReadRepository;
        private readonly IItemSearchReadModelConversionService _itemSearchReadModelConversionService;
        private readonly CancellationToken _cancellationToken;

        public ItemQueryService(IItemRepository itemRepository, IShoppingListRepository shoppingListRepository,
            IStoreRepository storeRepository, IItemTypeReadRepository itemTypeReadRepository,
            IItemSearchReadModelConversionService itemSearchReadModelConversionService,
            CancellationToken cancellationToken)
        {
            _itemRepository = itemRepository;
            _shoppingListRepository = shoppingListRepository;
            _storeRepository = storeRepository;
            _itemTypeReadRepository = itemTypeReadRepository;
            _itemSearchReadModelConversionService = itemSearchReadModelConversionService;
            _cancellationToken = cancellationToken;
        }

        public async Task<IEnumerable<ItemSearchReadModel>> SearchAsync(string name, StoreId storeId)
        {
            if (storeId is null)
                throw new ArgumentNullException(nameof(storeId));
            if (name == null) 
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name))
                return Enumerable.Empty<ItemSearchReadModel>();

            var store = await LoadStoreAsync(storeId);

            var searchResultItemGroups = (await _itemRepository
                .FindActiveByAsync(name.Trim(), storeId, _cancellationToken))
                .ToLookup(i => i.HasItemTypes);
            IShoppingList shoppingList = await LoadShoppingListAsync(storeId);

            var itemIdsOnShoppingListGroups = shoppingList.Items
                .Select(item => (item.Id, item.TypeId))
                .ToLookup(tuple => tuple.TypeId == null);

            // items without types
            var searchResultItemsWithoutTypes = searchResultItemGroups[false];
            var itemsWithoutTypesOnShoppingList = itemIdsOnShoppingListGroups[true].Select(tuple => tuple.Id);
            var itemsWithoutTypesNotOnShoppingList = searchResultItemsWithoutTypes
                .Where(item => !itemsWithoutTypesOnShoppingList.Contains(item.Id));

            var itemWithoutTypesReadModels = await _itemSearchReadModelConversionService.ConvertAsync(
                itemsWithoutTypesNotOnShoppingList, store, _cancellationToken);

            // items with types
            var searchResultItemsWithTypesDict = searchResultItemGroups[true].ToDictionary(g => g.Id);
            IEnumerable<(ItemId Id, ItemTypeId TypeId)> itemIdsWithTypeIdOnShoppingListGroups =
                itemIdsOnShoppingListGroups[false]!;
            var itemsWithTypeNotOnShoppingList = GetMatchingItemsWithTypeIds(storeId,
                    searchResultItemsWithTypesDict.Values, itemIdsWithTypeIdOnShoppingListGroups)
                .ToList();

            // types
            var itemsWithMatchingItemTypes = await GetItemsWithMatchingItemTypeIdsAsync(name, storeId,
                searchResultItemsWithTypesDict, itemIdsWithTypeIdOnShoppingListGroups.Select(m => m.TypeId));
            itemsWithTypeNotOnShoppingList.AddRange(itemsWithMatchingItemTypes);

            var itemsWithTypesReadModels = await _itemSearchReadModelConversionService.ConvertAsync(
                itemsWithTypeNotOnShoppingList, store, _cancellationToken);

            return itemsWithTypesReadModels.Union(itemWithoutTypesReadModels);
        }

        private IEnumerable<ItemWithMatchingItemTypeIds> GetMatchingItemsWithTypeIds(StoreId storeId,
            IEnumerable<IStoreItem> searchResultItemsWithTypes, IEnumerable<(ItemId, ItemTypeId)> itemIdsOnShoppingList)
        {
            var itemsWithTypesOnShoppingList = itemIdsOnShoppingList.ToLookup(g => g.Item1, g => g.Item2!);
            foreach (var item in searchResultItemsWithTypes)
            {
                if (!itemsWithTypesOnShoppingList.Contains(item.Id))
                {
                    var itemTypeIds = item.ItemTypes.GetForStore(storeId).Select(t => t.Id);
                    if (!itemTypeIds.Any())
                        continue;

                    yield return new ItemWithMatchingItemTypeIds(item, itemTypeIds);
                    continue;
                }

                var typeIdsOnList = itemsWithTypesOnShoppingList[item.Id].ToList();
                var typeIdsNotOnList = item.ItemTypes
                    .GetForStore(storeId)
                    .Select(t => t.Id)
                    .Except(typeIdsOnList)
                    .ToList();

                if (!typeIdsNotOnList.Any())
                    continue;

                yield return new ItemWithMatchingItemTypeIds(item, typeIdsNotOnList);
            }
        }

        private async Task<IEnumerable<ItemWithMatchingItemTypeIds>> GetItemsWithMatchingItemTypeIdsAsync(
            string name, StoreId storeId, Dictionary<ItemId, IStoreItem> searchResultItemsWithTypesDict,
            IEnumerable<ItemTypeId> itemTypeIdsOnShoppingList)
        {
            var itemTypeIdsOnShoppingListDict = itemTypeIdsOnShoppingList.ToHashSet();
            var itemTypeIdMappings =
                (await _itemTypeReadRepository.FindActiveByAsync(name, storeId, _cancellationToken)).ToList();
            if (!itemTypeIdMappings.Any())
                return Enumerable.Empty<ItemWithMatchingItemTypeIds>();

            var itemTypeIdGroups = itemTypeIdMappings
                .Where(mapping => !itemTypeIdsOnShoppingListDict.Contains(mapping.Item2))
                .GroupBy(mapping => mapping.Item1, mapping => mapping.Item2)
                // de-duplication => sort out all items that were already considered
                .Where(group => !searchResultItemsWithTypesDict.ContainsKey(group.Key))
                .ToList();

            var itemIds = itemTypeIdGroups.Select(group => group.Key);
            var itemsDict = (await _itemRepository.FindByAsync(itemIds, _cancellationToken))
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

        private async Task<IShoppingList> LoadShoppingListAsync(StoreId storeId)
        {
            IShoppingList? shoppingList = await _shoppingListRepository
                .FindActiveByAsync(storeId, _cancellationToken);
            if (shoppingList is null)
                throw new DomainException(new ShoppingListNotFoundReason(storeId));

            return shoppingList;
        }

        private async Task<IStore> LoadStoreAsync(StoreId storeId)
        {
            var store = await _storeRepository.FindByAsync(storeId, _cancellationToken);
            if (store == null)
                throw new DomainException(new StoreNotFoundReason(storeId));

            return store;
        }
    }
}