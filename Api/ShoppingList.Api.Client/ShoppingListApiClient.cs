using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.ItemCategory.Queries.AllActiveItemCategories;
using ProjectHermes.ShoppingList.Api.Contracts.Manufacturer.Queries.AllActiveManufacturers;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemToShoppingList;
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
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.ItemFilterResults;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.ItemSearch;
using RestEase;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Client
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

        public async Task<bool> IsAlive()
        {
            return await apiClient.IsAlive();
        }

        public async Task<ShoppingListContract> GetActiveShoppingListByStoreId(int storeId)
        {
            return await apiClient.GetActiveShoppingListByStoreId(storeId);
        }

        public async Task RemoveItemFromShoppingList(RemoveItemFromShoppingListContract contract)
        {
            await apiClient.RemoveItemFromShoppingList(contract);
        }

        public async Task AddItemToShoppingList(AddItemToShoppingListContract contract)
        {
            await apiClient.AddItemToShoppingList(contract);
        }

        public async Task PutItemInBasket(PutItemInBasketContract contract)
        {
            await apiClient.PutItemInBasket(contract);
        }

        public async Task RemoveItemFromBasket(RemoveItemFromBasketContract contract)
        {
            await apiClient.RemoveItemFromBasket(contract);
        }

        public async Task ChangeItemQuantityOnShoppingList(ChangeItemQuantityOnShoppingListContract contract)
        {
            await apiClient.ChangeItemQuantityOnShoppingList(contract);
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

        public async Task<IEnumerable<QuantityTypeInPacketContract>> GetAllQuantityTypesInPacket()
        {
            return await apiClient.GetAllQuantityTypesInPacket();
        }

        #endregion ShoppingListController

        #region ItemController

        public async Task CreateItem(CreateItemContract createItemContract)
        {
            await apiClient.CreateItem(createItemContract);
        }

        public async Task ChangeItem(ChangeItemContract changeItemContract)
        {
            await apiClient.ChangeItem(changeItemContract);
        }

        public async Task UpdateItemAsync(UpdateItemContract updateItemContract)
        {
            await apiClient.UpdateItemAsync(updateItemContract);
        }

        public async Task DeleteItemAsync(int itemId)
        {
            await apiClient.DeleteItemAsync(itemId);
        }

        public async Task<IEnumerable<ItemSearchContract>> GetItemSearchResults(string searchInput, int storeId)
        {
            return await apiClient.GetItemSearchResults(searchInput, storeId);
        }

        public async Task<IEnumerable<ItemFilterResultContract>> GetItemFilterResult(IEnumerable<int> storeIds,
            IEnumerable<int> itemCategoryIds, IEnumerable<int> manufacturerIds)
        {
            return await apiClient.GetItemFilterResult(storeIds, itemCategoryIds, manufacturerIds);
        }

        public async Task<StoreItemContract> Get(int itemId)
        {
            return await apiClient.Get(itemId);
        }

        public async Task CreateTemporaryItem(CreateTemporaryItemContract contract)
        {
            await apiClient.CreateTemporaryItem(contract);
        }

        public async Task MakeTemporaryItemPermanent(MakeTemporaryItemPermanentContract contract)
        {
            await apiClient.MakeTemporaryItemPermanent(contract);
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

        public async Task<IEnumerable<ActiveManufacturerContract>> GetAllActiveManufacturers()
        {
            return await apiClient.GetAllActiveManufacturers();
        }

        public async Task CreateManufacturer(string name)
        {
            await apiClient.CreateManufacturer(name);
        }

        #endregion ManufacturerController

        #region ItemCategoryController

        public async Task<IEnumerable<ItemCategoryContract>> GetItemCategorySearchResults(string searchInput)
        {
            return await apiClient.GetItemCategorySearchResults(searchInput);
        }

        public async Task<IEnumerable<ActiveItemCategoryContract>> GetAllActiveItemCategories()
        {
            return await apiClient.GetAllActiveItemCategories();
        }

        public async Task CreateItemCategory(string name)
        {
            await apiClient.CreateItemCategory(name);
        }

        #endregion ItemCategoryController
    }
}