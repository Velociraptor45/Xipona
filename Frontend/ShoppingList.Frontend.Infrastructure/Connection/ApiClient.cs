using ProjectHermes.ShoppingList.Api.Client;
using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.Manufacturers.Commands;
using ProjectHermes.ShoppingList.Api.Contracts.Manufacturers.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.SearchItemsForShoppingLists;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.Items;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.Manufacturers;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.ShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.Stores;
using ProjectHermes.ShoppingList.Frontend.Models.ItemCategories.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Manufacturers.Models;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection
{
    public class ApiClient : IApiClient
    {
        private readonly IShoppingListApiClient _client;
        private readonly IApiConverters _converters;

        public ApiClient(IShoppingListApiClient client, IApiConverters converters)
        {
            _client = client;
            _converters = converters;
        }

        public async Task IsAliveAsync()
        {
            _ = await _client.IsAlive();
        }

        public async Task PutItemInBasketAsync(PutItemInBasketRequest request)
        {
            var contract = _converters.ToContract<PutItemInBasketRequest, PutItemInBasketContract>(request);
            await _client.PutItemInBasketAsync(request.ShoppingListId, contract);
        }

        public async Task RemoveItemFromBasketAsync(RemoveItemFromBasketRequest request)
        {
            var contract = _converters.ToContract<RemoveItemFromBasketRequest, RemoveItemFromBasketContract>(request);
            await _client.RemoveItemFromBasketAsync(request.ShoppingListId, contract);
        }

        public async Task ChangeItemQuantityOnShoppingListAsync(ChangeItemQuantityOnShoppingListRequest request)
        {
            var contract =
                _converters
                    .ToContract<ChangeItemQuantityOnShoppingListRequest, ChangeItemQuantityOnShoppingListContract>(
                        request);
            await _client.ChangeItemQuantityOnShoppingListAsync(request.ShoppingListId, contract);
        }

        public async Task FinishListAsync(FinishListRequest request)
        {
            await _client.FinishListAsync(request.ShoppingListId);
        }

        public async Task RemoveItemFromShoppingListAsync(RemoveItemFromShoppingListRequest request)
        {
            var contract =
                _converters.ToContract<RemoveItemFromShoppingListRequest, RemoveItemFromShoppingListContract>(request);
            await _client.RemoveItemFromShoppingListAsync(request.ShoppingListId, contract);
        }

        public async Task AddItemToShoppingListAsync(AddItemToShoppingListRequest request)
        {
            var contract = _converters.ToContract<AddItemToShoppingListRequest, AddItemToShoppingListContract>(request);
            await _client.AddItemToShoppingListAsync(request.ShoppingListId, contract);
        }

        public async Task AddItemWithTypeToShoppingListAsync(AddItemWithTypeToShoppingListRequest request)
        {
            var contract =
                _converters.ToContract<AddItemWithTypeToShoppingListRequest, AddItemWithTypeToShoppingListContract>(
                    request);
            await _client.AddItemWithTypeToShoppingListAsync(request.ShoppingListId, request.ItemId, request.ItemTypeId,
                contract);
        }

        public async Task UpdateItemAsync(UpdateItemRequest request)
        {
            var contract = _converters.ToContract<Item, UpdateItemContract>(request.StoreItem);
            await _client.UpdateItemAsync(request.StoreItem.Id, contract);
        }

        public async Task UpdateItemWithTypesAsync(UpdateItemWithTypesRequest request)
        {
            var contract = _converters.ToContract<Item, UpdateItemWithTypesContract>(request.StoreItem);
            await _client.UpdateItemWithTypesAsync(request.StoreItem.Id, contract);
        }

        public async Task ModifyItemAsync(ModifyItemRequest request)
        {
            var contract = _converters.ToContract<Item, ModifyItemContract>(request.StoreItem);
            await _client.ModifyItemAsync(request.StoreItem.Id, contract);
        }

        public async Task ModifyItemWithTypesAsync(ModifyItemWithTypesRequest request)
        {
            var contract = _converters.ToContract<ModifyItemWithTypesRequest, ModifyItemWithTypesContract>(request);
            await _client.ModifyItemWithTypesAsync(request.StoreItem.Id, contract);
        }

        public async Task CreateItemAsync(CreateItemRequest request)
        {
            var contract = _converters.ToContract<Item, CreateItemContract>(request.StoreItem);
            await _client.CreateItemAsync(contract);
        }

        public async Task CreateItemWithTypesAsync(CreateItemWithTypesRequest request)
        {
            var contract = _converters.ToContract<Item, CreateItemWithTypesContract>(request.StoreItem);
            await _client.CreateItemWithTypesAsync(contract);
        }

        public async Task DeleteItemAsync(DeleteItemRequest request)
        {
            await _client.DeleteItemAsync(request.ItemId);
        }

        public async Task<Manufacturer> CreateManufacturerAsync(string name)
        {
            var result = await _client.CreateManufacturerAsync(name);
            return _converters.ToDomain<ManufacturerContract, Manufacturer>(result);
        }

        public async Task CreateItemCategoryAsync(string name)
        {
            await _client.CreateItemCategory(name);
        }

        public async Task<ShoppingListRoot> GetActiveShoppingListByStoreIdAsync(Guid storeId)
        {
            var list = await _client.GetActiveShoppingListByStoreIdAsync(storeId);
            return _converters.ToDomain<ShoppingListContract, ShoppingListRoot>(list);
        }

        public async Task<IEnumerable<Store>> GetAllActiveStoresAsync()
        {
            var contracts = await _client.GetAllActiveStoresAsync();

            return contracts is null ?
                Enumerable.Empty<Store>() :
                contracts.Select(_converters.ToDomain<ActiveStoreContract, Store>);
        }

        public async Task<IEnumerable<Manufacturer>> GetAllActiveManufacturersAsync()
        {
            var manufacturers = await _client.GetAllActiveManufacturersAsync();

            return manufacturers is null ?
                Enumerable.Empty<Manufacturer>() :
                manufacturers.Select(_converters.ToDomain<ManufacturerContract, Manufacturer>);
        }

        public async Task<IEnumerable<ItemCategory>> GetAllActiveItemCategoriesAsync()
        {
            var itemCategories = await _client.GetAllActiveItemCategories();

            if (itemCategories is null)
                return Enumerable.Empty<ItemCategory>();

            return itemCategories.Select(_converters.ToDomain<ItemCategoryContract, ItemCategory>);
        }

        public async Task<IEnumerable<SearchItemForShoppingListResult>> SearchItemsForShoppingListAsync(
            string searchInput, Guid storeId)
        {
            var result = await _client.SearchItemsForShoppingListAsync(storeId, searchInput);
            if (result is null)
                return Enumerable.Empty<SearchItemForShoppingListResult>();

            return result
                .Select(_converters.ToDomain<SearchItemForShoppingListResultContract, SearchItemForShoppingListResult>);
        }

        public async Task<IEnumerable<SearchItemResult>> SearchItemsAsync(string searchInput)
        {
            var result = await _client.SearchItemsAsync(searchInput);

            return result is null ?
                Enumerable.Empty<SearchItemResult>() :
                result.Select(_converters.ToDomain<SearchItemResultContract, SearchItemResult>);
        }

        public async Task<IEnumerable<SearchItemResult>> SearchItemsByFilterAsync(IEnumerable<Guid> storeIds,
            IEnumerable<Guid> itemCategoryIds, IEnumerable<Guid> manufacturerIds)
        {
            var result = await _client.SearchItemsByFilterAsync(
                storeIds,
                itemCategoryIds,
                manufacturerIds);

            return result is null ?
                Enumerable.Empty<SearchItemResult>() :
                result.Select(_converters.ToDomain<SearchItemResultContract, SearchItemResult>);
        }

        public async Task<Item> GetItemByIdAsync(Guid itemId)
        {
            var result = await _client.GetAsync(itemId);
            return _converters.ToDomain<StoreItemContract, Item>(result);
        }

        public async Task<IEnumerable<QuantityType>> GetAllQuantityTypesAsync()
        {
            var result = await _client.GetAllQuantityTypesAsync();
            return result is null ?
                Enumerable.Empty<QuantityType>() :
                result.Select(_converters.ToDomain<QuantityTypeContract, QuantityType>);
        }

        public async Task<IEnumerable<QuantityTypeInPacket>> GetAllQuantityTypesInPacketAsync()
        {
            var result = await _client.GetAllQuantityTypesInPacketAsync();
            return result is null ?
                Enumerable.Empty<QuantityTypeInPacket>() :
                result.Select(_converters.ToDomain<QuantityTypeInPacketContract, QuantityTypeInPacket>);
        }

        public async Task CreateTemporaryItem(CreateTemporaryItemRequest request)
        {
            var contract = _converters.ToContract<CreateTemporaryItemRequest, CreateTemporaryItemContract>(request);
            await _client.CreateTemporaryItemAsync(contract);
        }

        public async Task MakeTemporaryItemPermanent(MakeTemporaryItemPermanentRequest request)
        {
            var contract =
                _converters.ToContract<MakeTemporaryItemPermanentRequest, MakeTemporaryItemPermanentContract>(request);
            await _client.MakeTemporaryItemPermanentAsync(request.ItemId, contract);
        }

        public async Task CreateStoreAsync(CreateStoreRequest request)
        {
            var contract = _converters.ToContract<CreateStoreRequest, CreateStoreContract>(request);
            await _client.CreateStoreAsync(contract);
        }

        public async Task ModifyStoreAsync(ModifyStoreRequest request)
        {
            await _client.UpdateStoreAsync(_converters.ToContract<ModifyStoreRequest, UpdateStoreContract>(request));
        }

        public async Task<IEnumerable<ManufacturerSearchResult>> GetManufacturerSearchResultsAsync(string searchInput)
        {
            var result = await _client.GetManufacturerSearchResultsAsync(searchInput);

            return result is null
                ? Enumerable.Empty<ManufacturerSearchResult>()
                : result.Select(_converters.ToDomain<ManufacturerSearchResultContract, ManufacturerSearchResult>);
        }

        public async Task<Manufacturer> GetManufacturerByIdAsync(Guid id)
        {
            var result = await _client.GetManufacturerByIdAsync(id);

            return _converters.ToDomain<ManufacturerContract, Manufacturer>(result);
        }

        public async Task DeleteManufacturerAsync(Guid id)
        {
            await _client.DeleteManufacturerAsync(id);
        }

        public async Task ModifyManufacturerAsync(ModifyManufacturerRequest request)
        {
            var contract = _converters.ToContract<ModifyManufacturerRequest, ModifyManufacturerContract>(request);
            await _client.ModifyManufacturerAsync(contract);
        }
    }
}