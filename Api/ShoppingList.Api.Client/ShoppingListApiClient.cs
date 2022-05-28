using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.SearchItemsForShoppingLists;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Shared;
using RestEase;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Client
{
    public class ShoppingListApiClient : IShoppingListApiClient
    {
        private readonly IShoppingListApiClient _apiClient;

        public ShoppingListApiClient(HttpClient httpClient)
        {
            _apiClient = new RestClient(httpClient)
            {
                JsonSerializerSettings =
                new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            }.For<IShoppingListApiClient>();
        }

        public async Task<bool> IsAlive()
        {
            return await _apiClient.IsAlive();
        }

        #region ShoppingListController

        public async Task<ShoppingListContract> GetActiveShoppingListByStoreIdAsync(Guid storeId)
        {
            return await _apiClient.GetActiveShoppingListByStoreIdAsync(storeId);
        }

        public async Task RemoveItemFromShoppingListAsync(Guid id,
            RemoveItemFromShoppingListContract contract)
        {
            await _apiClient.RemoveItemFromShoppingListAsync(id, contract);
        }

        public async Task AddItemToShoppingListAsync(Guid id, AddItemToShoppingListContract contract)
        {
            await _apiClient.AddItemToShoppingListAsync(id, contract);
        }

        public async Task AddItemWithTypeToShoppingListAsync(Guid id, Guid itemId, Guid itemTypeId,
            AddItemWithTypeToShoppingListContract contract)
        {
            await _apiClient.AddItemWithTypeToShoppingListAsync(id, itemId, itemTypeId, contract);
        }

        public async Task PutItemInBasketAsync(Guid id, PutItemInBasketContract contract)
        {
            await _apiClient.PutItemInBasketAsync(id, contract);
        }

        public async Task RemoveItemFromBasketAsync(Guid id, RemoveItemFromBasketContract contract)
        {
            await _apiClient.RemoveItemFromBasketAsync(id, contract);
        }

        public async Task ChangeItemQuantityOnShoppingListAsync(Guid id, ChangeItemQuantityOnShoppingListContract contract)
        {
            await _apiClient.ChangeItemQuantityOnShoppingListAsync(id, contract);
        }

        public async Task FinishListAsync(Guid id)
        {
            await _apiClient.FinishListAsync(id);
        }

        #endregion ShoppingListController

        #region ItemController

        public async Task CreateItemAsync(CreateItemContract contract)
        {
            await _apiClient.CreateItemAsync(contract);
        }

        public async Task CreateItemWithTypesAsync(CreateItemWithTypesContract contract)
        {
            await _apiClient.CreateItemWithTypesAsync(contract);
        }

        public async Task ModifyItemAsync(Guid id, ModifyItemContract contract)
        {
            await _apiClient.ModifyItemAsync(id, contract);
        }

        public async Task ModifyItemWithTypesAsync(Guid id, ModifyItemWithTypesContract contract)
        {
            await _apiClient.ModifyItemWithTypesAsync(id, contract);
        }

        public async Task UpdateItemAsync(Guid id, UpdateItemContract contract)
        {
            await _apiClient.UpdateItemAsync(id, contract);
        }

        public async Task UpdateItemWithTypesAsync(Guid id, UpdateItemWithTypesContract contract)
        {
            await _apiClient.UpdateItemWithTypesAsync(id, contract);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            await _apiClient.DeleteItemAsync(id);
        }

        public async Task<IEnumerable<SearchItemForShoppingListResultContract>> SearchItemsForShoppingListAsync(
            Guid storeId, string searchInput)
        {
            return await _apiClient.SearchItemsForShoppingListAsync(storeId, searchInput);
        }

        public async Task<IEnumerable<SearchItemResultContract>> SearchItemsAsync(string searchInput)
        {
            return await _apiClient.SearchItemsAsync(searchInput);
        }

        public async Task<IEnumerable<SearchItemResultContract>> SearchItemsByFilterAsync(IEnumerable<Guid> storeIds,
            IEnumerable<Guid> itemCategoryIds, IEnumerable<Guid> manufacturerIds)
        {
            return await _apiClient.SearchItemsByFilterAsync(storeIds, itemCategoryIds, manufacturerIds);
        }

        public async Task<StoreItemContract> GetAsync(Guid id)
        {
            return await _apiClient.GetAsync(id);
        }

        public async Task CreateTemporaryItemAsync(CreateTemporaryItemContract contract)
        {
            await _apiClient.CreateTemporaryItemAsync(contract);
        }

        public async Task MakeTemporaryItemPermanentAsync(Guid id, MakeTemporaryItemPermanentContract contract)
        {
            await _apiClient.MakeTemporaryItemPermanentAsync(id, contract);
        }

        public async Task<IEnumerable<QuantityTypeContract>> GetAllQuantityTypesAsync()
        {
            return await _apiClient.GetAllQuantityTypesAsync();
        }

        public async Task<IEnumerable<QuantityTypeInPacketContract>> GetAllQuantityTypesInPacketAsync()
        {
            return await _apiClient.GetAllQuantityTypesInPacketAsync();
        }

        #endregion ItemController

        #region StoreController

        public async Task<IEnumerable<ActiveStoreContract>> GetAllActiveStoresAsync()
        {
            return await _apiClient.GetAllActiveStoresAsync();
        }

        public async Task<StoreContract> CreateStoreAsync(CreateStoreContract createStoreContract)
        {
            return await _apiClient.CreateStoreAsync(createStoreContract);
        }

        public async Task UpdateStoreAsync(UpdateStoreContract updateStoreContract)
        {
            await _apiClient.UpdateStoreAsync(updateStoreContract);
        }

        #endregion StoreController

        #region ManufacturerController

        public async Task<ManufacturerContract> GetManufacturerByIdAsync(Guid id)
        {
            return await _apiClient.GetManufacturerByIdAsync(id);
        }

        public async Task<IEnumerable<ManufacturerSearchResultContract>> GetManufacturerSearchResultsAsync(
            string searchInput, bool includeDeleted)
        {
            return await _apiClient.GetManufacturerSearchResultsAsync(searchInput, includeDeleted);
        }

        public async Task<IEnumerable<ManufacturerContract>> GetAllActiveManufacturersAsync()
        {
            return await _apiClient.GetAllActiveManufacturersAsync();
        }

        public async Task<ManufacturerContract> CreateManufacturerAsync(string name)
        {
            return await _apiClient.CreateManufacturerAsync(name);
        }

        public async Task DeleteManufacturerAsync(Guid id)
        {
            await _apiClient.DeleteManufacturerAsync(id);
        }

        public async Task ModifyManufacturerAsync(ModifyManufacturerContract contract)
        {
            await _apiClient.ModifyManufacturerAsync(contract);
        }

        #endregion ManufacturerController

        #region ItemCategoryController

        public async Task<IEnumerable<ItemCategoryContract>> SearchItemCategoriesByName(string searchInput)
        {
            return await _apiClient.SearchItemCategoriesByName(searchInput);
        }

        public async Task<IEnumerable<ItemCategoryContract>> GetAllActiveItemCategories()
        {
            return await _apiClient.GetAllActiveItemCategories();
        }

        public async Task<ItemCategoryContract> CreateItemCategory(string name)
        {
            return await _apiClient.CreateItemCategory(name);
        }

        public async Task DeleteItemCategory(Guid id)
        {
            await _apiClient.DeleteItemCategory(id);
        }

        #endregion ItemCategoryController
    }
}