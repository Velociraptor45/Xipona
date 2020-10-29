using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestEase;
using ShoppingList.Api.Contracts.Commands.CreateItem;
using ShoppingList.Api.Contracts.Commands.CreateStore;
using ShoppingList.Api.Contracts.Commands.UpdateItem;
using ShoppingList.Api.Contracts.Commands.UpdateStore;
using ShoppingList.Api.Contracts.Queries;
using ShoppingList.Api.Contracts.Queries.AllActiveStores;
using ShoppingList.Api.Contracts.Queries.AllQuantityTypes;
using ShoppingList.Api.Contracts.SharedContracts;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShoppingList.Api.Client
{
    public class ShoppingListApiClient : IShoppingListApiClient
    {
        private readonly IShoppingListApiClient apiClient;

        public ShoppingListApiClient(HttpClient httpClient)
        {
            apiClient = new RestClient(httpClient)
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

        public async Task<ShoppingListContract> GetActiveShoppingListByStoreId(int storeId)
        {
            return await apiClient.GetActiveShoppingListByStoreId(storeId);
        }

        public async Task RemoveItemFromShoppingList(int shoppingListId, int itemId)
        {
            await apiClient.RemoveItemFromShoppingList(shoppingListId, itemId);
        }

        public async Task AddItemToShoppingList(int shoppingListId, int itemId, float quantity)
        {
            await apiClient.AddItemToShoppingList(shoppingListId, itemId, quantity);
        }

        public async Task PutItemInBasket(int shoppingListId, int itemId)
        {
            await apiClient.PutItemInBasket(shoppingListId, itemId);
        }

        public async Task RemoveItemFromBasket(int shoppingListId, int itemId)
        {
            await apiClient.RemoveItemFromBasket(shoppingListId, itemId);
        }

        public async Task ChangeItemQuantityOnShoppingList(int shoppingListId, int itemId, float quantity)
        {
            await apiClient.ChangeItemQuantityOnShoppingList(shoppingListId, itemId, quantity);
        }

        public async Task CreatList(int storeId)
        {
            await apiClient.CreatList(storeId);
        }

        public async Task FinishList(int shoppingListId)
        {
            await apiClient.FinishList(shoppingListId);
        }

        public async Task<IEnumerable<QuantityTypeContract>> GetAllQuantityTypes()
        {
            return await apiClient.GetAllQuantityTypes();
        }

        public async Task<IEnumerable<QuantityInPacketTypeContract>> GetAllQuantityInPacketTypes()
        {
            return await apiClient.GetAllQuantityInPacketTypes();
        }

        #endregion ShoppingListController

        #region ItemController

        public async Task CreateItem(CreateItemContract createItemContract)
        {
            await apiClient.CreateItem(createItemContract);
        }

        public async Task UpdateItem(UpdateItemContract updateItemContract)
        {
            await apiClient.UpdateItem(updateItemContract);
        }

        public async Task<IEnumerable<ItemSearchContract>> GetItemSearchResults(string searchInput, int storeId)
        {
            return await apiClient.GetItemSearchResults(searchInput, storeId);
        }

        #endregion ItemController

        #region StoreController

        public async Task<IEnumerable<ActiveStoreContract>> GetAllActiveStores()
        {
            return await apiClient.GetAllActiveStores();
        }

        public async Task CreateStore(CreateStoreContract createStoreContract)
        {
            await apiClient.CreateStore(createStoreContract);
        }

        public async Task UpdateStore(UpdateStoreContract updateStoreContract)
        {
            await apiClient.UpdateStore(updateStoreContract);
        }

        #endregion StoreController

        #region ManufacturerController

        public async Task<IEnumerable<ManufacturerContract>> GetManufacturerSearchResults(string searchInput)
        {
            return await apiClient.GetManufacturerSearchResults(searchInput);
        }

        #endregion ManufacturerController

        #region ItemCategoryController

        public async Task<IEnumerable<ManufacturerContract>> GetItemCategorySearchResults(string searchInput)
        {
            return await apiClient.GetItemCategorySearchResults(searchInput);
        }

        #endregion ItemCategoryController
    }
}