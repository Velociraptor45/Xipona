using ProjectHermes.ShoppingList.Api.Client;
using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.SearchItemsForShoppingLists;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Models;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Requests;
using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Index.Search;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
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
            await _client.PutItemInBasket(request.ToContract());
        }

        public async Task RemoveItemFromBasketAsync(RemoveItemFromBasketRequest request)
        {
            await _client.RemoveItemFromBasket(request.ToContract());
        }

        public async Task ChangeItemQuantityOnShoppingListAsync(ChangeItemQuantityOnShoppingListRequest request)
        {
            await _client.ChangeItemQuantityOnShoppingList(request.ToContract());
        }

        public async Task FinishListAsync(FinishListRequest request)
        {
            await _client.FinishList(request.ShoppingListId);
        }

        public async Task RemoveItemFromShoppingListAsync(RemoveItemFromShoppingListRequest request)
        {
            var contract =
                _converters.ToContract<RemoveItemFromShoppingListRequest, RemoveItemFromShoppingListContract>(request);
            await _client.RemoveItemFromShoppingList(contract);
        }

        public async Task AddItemToShoppingListAsync(AddItemToShoppingListRequest request)
        {
            var contract = _converters.ToContract<AddItemToShoppingListRequest, AddItemToShoppingListContract>(request);
            await _client.AddItemToShoppingList(contract);
        }

        public async Task AddItemWithTypeToShoppingListAsync(AddItemWithTypeToShoppingListRequest request)
        {
            await _client.AddItemWithTypeToShoppingList(request.ToContract());
        }

        public async Task UpdateItemAsync(UpdateItemRequest request)
        {
            await _client.UpdateItemAsync(request.StoreItem.ToUpdateItemContract());
        }

        public async Task UpdateItemWithTypesAsync(UpdateItemWithTypesRequest request)
        {
            var contract = request.StoreItem.ToUpdateItemWithTypesContract();
            await _client.UpdateItemWithTypesAsync(contract);
        }

        public async Task ModifyItemAsync(ModifyItemRequest request)
        {
            await _client.ModifyItem(request.StoreItem.ToModifyItemContract());
        }

        public async Task ModifyItemWithTypesAsync(ModifyItemWithTypesRequest request)
        {
            var contract = _converters.ToContract<ModifyItemWithTypesRequest, ModifyItemWithTypesContract>(request);
            await _client.ModifyItemWithTypesAsync(contract);
        }

        public async Task CreateItemAsync(CreateItemRequest request)
        {
            await _client.CreateItem(request.StoreItem.ToCreateItemContract());
        }

        public async Task CreateItemWithTypesAsync(CreateItemWithTypesRequest request)
        {
            await _client.CreateItemWithTypes(request.StoreItem.ToCreateItemWithTypesContract());
        }

        public async Task DeleteItemAsync(DeleteItemRequest request)
        {
            await _client.DeleteItemAsync(request.ItemId);
        }

        public async Task CreateManufacturerAsync(string name)
        {
            await _client.CreateManufacturer(name);
        }

        public async Task CreateItemCategoryAsync(string name)
        {
            await _client.CreateItemCategory(name);
        }

        public async Task<ShoppingListRoot> GetActiveShoppingListByStoreIdAsync(Guid storeId)
        {
            var list = await _client.GetActiveShoppingListByStoreId(storeId);
            return _converters.ToDomain<ShoppingListContract, ShoppingListRoot>(list);
        }

        public async Task<IEnumerable<Store>> GetAllActiveStoresAsync()
        {
            var contracts = await _client.GetAllActiveStores();
            return contracts.Select(_converters.ToDomain<ActiveStoreContract, Store>);
        }

        public async Task<IEnumerable<Manufacturer>> GetAllActiveManufacturersAsync()
        {
            var manufacturers = await _client.GetAllActiveManufacturers();

            return manufacturers.Select(_converters.ToDomain<ManufacturerContract, Manufacturer>);
        }

        public async Task<IEnumerable<ItemCategory>> GetAllActiveItemCategoriesAsync()
        {
            var itemCategories = await _client.GetAllActiveItemCategories();

            return itemCategories.Select(_converters.ToDomain<ItemCategoryContract, ItemCategory>);
        }

        public async Task<IEnumerable<SearchItemForShoppingListResult>> SearchItemsForShoppingListAsync(
            string searchInput, Guid storeId)
        {
            var result = await _client.SearchItemsForShoppingListAsync(searchInput, storeId);
            return result
                .Select(_converters.ToDomain<SearchItemForShoppingListResultContract, SearchItemForShoppingListResult>);
        }

        public async Task<IEnumerable<SearchItemResult>> SearchItemsAsync(string searchInput)
        {
            var result = await _client.SearchItemsAsync(searchInput) ?? Enumerable.Empty<SearchItemResultContract>();
            return result.Select(_converters.ToDomain<SearchItemResultContract, SearchItemResult>);
        }

        public async Task<IEnumerable<SearchItemResult>> SearchItemsByFilterAsync(IEnumerable<Guid> storeIds,
            IEnumerable<Guid> itemCategoryIds, IEnumerable<Guid> manufacturerIds)
        {
            var result = await _client.SearchItemsByFilterAsync(
                storeIds,
                itemCategoryIds,
                manufacturerIds);

            return result.Select(_converters.ToDomain<SearchItemResultContract, SearchItemResult>);
        }

        public async Task<StoreItem> GetItemByIdAsync(Guid itemId)
        {
            var result = await _client.Get(itemId);
            return _converters.ToDomain<StoreItemContract, StoreItem>(result);
        }

        public async Task<IEnumerable<QuantityType>> GetAllQuantityTypesAsync()
        {
            var result = await _client.GetAllQuantityTypes();
            return result.Select(_converters.ToDomain<QuantityTypeContract, QuantityType>);
        }

        public async Task<IEnumerable<QuantityTypeInPacket>> GetAllQuantityTypesInPacketAsync()
        {
            var result = await _client.GetAllQuantityTypesInPacket();
            return result.Select(_converters.ToDomain<QuantityTypeInPacketContract, QuantityTypeInPacket>);
        }

        public async Task CreateTemporaryItem(CreateTemporaryItemRequest request)
        {
            await _client.CreateTemporaryItem(request.ToContract());
        }

        public async Task MakeTemporaryItemPermanent(MakeTemporaryItemPermanentRequest request)
        {
            await _client.MakeTemporaryItemPermanent(request.ToContract());
        }

        public async Task CreateStoreAsync(CreateStoreRequest request)
        {
            var contract = _converters.ToContract<CreateStoreRequest, CreateStoreContract>(request);
            await _client.CreateStore(contract);
        }

        public async Task ModifyStoreAsync(ModifyStoreRequest request)
        {
            await _client.UpdateStore(_converters.ToContract<ModifyStoreRequest, UpdateStoreContract>(request));
        }
    }
}