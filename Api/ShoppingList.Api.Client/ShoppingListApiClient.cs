using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.ItemCategory.Commands;
using ProjectHermes.ShoppingList.Api.Contracts.ItemCategory.Queries.AllActiveItemCategories;
using ProjectHermes.ShoppingList.Api.Contracts.Manufacturer.Queries.AllActiveManufacturers;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
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

        #region ShoppingListController

        public async Task<bool> IsAlive()
        {
            return await _apiClient.IsAlive();
        }

        public async Task<ShoppingListContract> GetActiveShoppingListByStoreId(Guid storeId)
        {
            return await _apiClient.GetActiveShoppingListByStoreId(storeId);
        }

        public async Task RemoveItemFromShoppingList(RemoveItemFromShoppingListContract contract)
        {
            await _apiClient.RemoveItemFromShoppingList(contract);
        }

        public async Task AddItemToShoppingList(AddItemToShoppingListContract contract)
        {
            await _apiClient.AddItemToShoppingList(contract);
        }

        public async Task AddItemWithTypeToShoppingList(AddItemWithTypeToShoppingListContract contract)
        {
            await _apiClient.AddItemWithTypeToShoppingList(contract);
        }

        public async Task PutItemInBasket(PutItemInBasketContract contract)
        {
            await _apiClient.PutItemInBasket(contract);
        }

        public async Task RemoveItemFromBasket(RemoveItemFromBasketContract contract)
        {
            await _apiClient.RemoveItemFromBasket(contract);
        }

        public async Task ChangeItemQuantityOnShoppingList(ChangeItemQuantityOnShoppingListContract contract)
        {
            await _apiClient.ChangeItemQuantityOnShoppingList(contract);
        }

        public async Task FinishList(Guid shoppingListId)
        {
            await _apiClient.FinishList(shoppingListId);
        }

        public async Task<IEnumerable<QuantityTypeContract>> GetAllQuantityTypes()
        {
            return await _apiClient.GetAllQuantityTypes();
        }

        public async Task<IEnumerable<QuantityTypeInPacketContract>> GetAllQuantityTypesInPacket()
        {
            return await _apiClient.GetAllQuantityTypesInPacket();
        }

        #endregion ShoppingListController

        #region ItemController

        public async Task CreateItem(CreateItemContract createItemContract)
        {
            await _apiClient.CreateItem(createItemContract);
        }

        public async Task CreateItemWithTypes(CreateItemWithTypesContract createItemWithTypesContract)
        {
            await _apiClient.CreateItemWithTypes(createItemWithTypesContract);
        }

        public async Task ModifyItem(ModifyItemContract modifyItemContract)
        {
            await _apiClient.ModifyItem(modifyItemContract);
        }

        public async Task ModifyItemWithTypesAsync(ModifyItemWithTypesContract contract)
        {
            await _apiClient.ModifyItemWithTypesAsync(contract);
        }

        public async Task UpdateItemAsync(UpdateItemContract updateItemContract)
        {
            await _apiClient.UpdateItemAsync(updateItemContract);
        }

        public async Task UpdateItemWithTypesAsync(UpdateItemWithTypesContract contract)
        {
            await _apiClient.UpdateItemWithTypesAsync(contract);
        }

        public async Task DeleteItemAsync(Guid itemId)
        {
            await _apiClient.DeleteItemAsync(itemId);
        }

        public async Task<IEnumerable<SearchItemForShoppingListResultContract>> SearchItemsForShoppingListAsync(
            string searchInput, Guid storeId)
        {
            return await _apiClient.SearchItemsForShoppingListAsync(searchInput, storeId);
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

        public async Task<StoreItemContract> Get(Guid itemId)
        {
            return await _apiClient.Get(itemId);
        }

        public async Task CreateTemporaryItem(CreateTemporaryItemContract contract)
        {
            await _apiClient.CreateTemporaryItem(contract);
        }

        public async Task MakeTemporaryItemPermanent(MakeTemporaryItemPermanentContract contract)
        {
            await _apiClient.MakeTemporaryItemPermanent(contract);
        }

        #endregion ItemController

        #region StoreController

        public async Task<IEnumerable<ActiveStoreContract>> GetAllActiveStores()
        {
            return await _apiClient.GetAllActiveStores();
        }

        public async Task CreateStore(CreateStoreContract createStoreContract)
        {
            await _apiClient.CreateStore(createStoreContract);
        }

        public async Task UpdateStore(UpdateStoreContract updateStoreContract)
        {
            await _apiClient.UpdateStore(updateStoreContract);
        }

        #endregion StoreController

        #region ManufacturerController

        public async Task<IEnumerable<ManufacturerContract>> GetManufacturerSearchResults(string searchInput)
        {
            return await _apiClient.GetManufacturerSearchResults(searchInput);
        }

        public async Task<IEnumerable<ActiveManufacturerContract>> GetAllActiveManufacturers()
        {
            return await _apiClient.GetAllActiveManufacturers();
        }

        public async Task CreateManufacturer(string name)
        {
            await _apiClient.CreateManufacturer(name);
        }

        #endregion ManufacturerController

        #region ItemCategoryController

        public async Task<IEnumerable<ItemCategoryContract>> GetItemCategorySearchResults(string searchInput)
        {
            return await _apiClient.GetItemCategorySearchResults(searchInput);
        }

        public async Task<IEnumerable<ActiveItemCategoryContract>> GetAllActiveItemCategories()
        {
            return await _apiClient.GetAllActiveItemCategories();
        }

        public async Task CreateItemCategory(string name)
        {
            await _apiClient.CreateItemCategory(name);
        }

        public async Task DeleteItemCategory([Body] DeleteItemCategoryContract contract)
        {
            await _apiClient.DeleteItemCategory(contract);
        }

        #endregion ItemCategoryController
    }
}