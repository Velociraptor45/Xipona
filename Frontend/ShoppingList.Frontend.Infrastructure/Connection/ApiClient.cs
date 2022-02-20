using ProjectHermes.ShoppingList.Api.Client;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converter;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Models;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Requests;
using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Index.Search;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection
{
    public class ApiClient : IApiClient
    {
        private readonly IShoppingListApiClient client;

        public ApiClient(IShoppingListApiClient client)
        {
            this.client = client;
        }

        public async Task IsAliveAsync()
        {
            _ = await client.IsAlive();
        }

        public async Task PutItemInBasketAsync(PutItemInBasketRequest request)
        {
            await client.PutItemInBasket(request.ToContract());
        }

        public async Task RemoveItemFromBasketAsync(RemoveItemFromBasketRequest request)
        {
            await client.RemoveItemFromBasket(request.ToContract());
        }

        public async Task ChangeItemQuantityOnShoppingListAsync(ChangeItemQuantityOnShoppingListRequest request)
        {
            await client.ChangeItemQuantityOnShoppingList(request.ToContract());
        }

        public async Task FinishListAsync(FinishListRequest request)
        {
            await client.FinishList(request.ShoppingListId);
        }

        public async Task RemoveItemFromShoppingListAsync(RemoveItemFromShoppingListRequest request)
        {
            await client.RemoveItemFromShoppingList(request.ToContract());
        }

        public async Task AddItemToShoppingListAsync(AddItemToShoppingListRequest request)
        {
            await client.AddItemToShoppingList(request.ToContract());
        }

        public async Task AddItemWithTypeToShoppingListAsync(AddItemWithTypeToShoppingListRequest request)
        {
            await client.AddItemWithTypeToShoppingList(request.ToContract());
        }

        public async Task UpdateItemAsync(UpdateItemRequest request)
        {
            await client.UpdateItemAsync(request.StoreItem.ToUpdateItemContract());
        }

        public async Task UpdateItemWithTypesAsync(UpdateItemWithTypesRequest request)
        {
            var contract = request.StoreItem.ToUpdateItemWithTypesContract();
            await client.UpdateItemWithTypesAsync(contract);
        }

        public async Task ModifyItemAsync(ModifyItemRequest request)
        {
            await client.ModifyItem(request.StoreItem.ToModifyItemContract());
        }

        public async Task ModifyItemWithTypesAsync(ModifyItemWithTypesRequest request)
        {
            await client.ModifyItemWithTypesAsync(request.StoreItem.ToModifyItemWithTypesContract());
        }

        public async Task CreateItemAsync(CreateItemRequest request)
        {
            await client.CreateItem(request.StoreItem.ToCreateItemContract());
        }

        public async Task CreateItemWithTypesAsync(CreateItemWithTypesRequest request)
        {
            await client.CreateItemWithTypes(request.StoreItem.ToCreateItemWithTypesContract());
        }

        public async Task DeleteItemAsync(DeleteItemRequest request)
        {
            await client.DeleteItemAsync(request.ItemId);
        }

        public async Task CreateManufacturerAsync(string name)
        {
            await client.CreateManufacturer(name);
        }

        public async Task CreateItemCategoryAsync(string name)
        {
            await client.CreateItemCategory(name);
        }

        public async Task<ShoppingListRoot> GetActiveShoppingListByStoreIdAsync(int storeId)
        {
            var list = await client.GetActiveShoppingListByStoreId(storeId);
            return list.ToModel();
        }

        public async Task<IEnumerable<Store>> GetAllActiveStoresAsync()
        {
            var stores = await client.GetAllActiveStores();
            return stores.Select(store => store.ToModel());
        }

        public async Task<IEnumerable<Manufacturer>> GetAllActiveManufacturersAsync()
        {
            var manufacturers = await client.GetAllActiveManufacturers();

            return manufacturers.Select(man => man.ToModel());
        }

        public async Task<IEnumerable<ItemCategory>> GetAllActiveItemCategoriesAsync()
        {
            var itemCategories = await client.GetAllActiveItemCategories();

            return itemCategories.Select(cat => cat.ToModel());
        }

        public async Task<IEnumerable<SearchItemForShoppingListResult>> GetItemSearchResultsAsync(string searchInput, int storeId)
        {
            var converter = new SearchItemForShoppingListResultConverter();

            var result = await client.SearchItemsForShoppingListAsync(searchInput, storeId);
            return result.Select(converter.ToDomain);
        }

        public async Task<IEnumerable<SearchItemResult>> GetItemFilterResultAsync(IEnumerable<int> storeIds,
            IEnumerable<int> itemCategoryIds, IEnumerable<int> manufacturerIds)
        {
            var converter = new SearchItemResultConverter();

            var result = await client.SearchItemsByFilterAsync(
                storeIds,
                itemCategoryIds,
                manufacturerIds);

            return result.Select(converter.ToDomain);
        }

        public async Task<StoreItem> GetItemByIdAsync(int itemId)
        {
            var result = await client.Get(itemId);
            return result.ToModel();
        }

        public async Task<IEnumerable<QuantityType>> GetAllQuantityTypesAsync()
        {
            var result = await client.GetAllQuantityTypes();
            return result.Select(r => r.ToModel());
        }

        public async Task<IEnumerable<QuantityTypeInPacket>> GetAllQuantityTypesInPacketAsync()
        {
            var result = await client.GetAllQuantityTypesInPacket();
            return result.Select(r => r.ToModel());
        }

        public async Task CreateTemporaryItem(CreateTemporaryItemRequest request)
        {
            await client.CreateTemporaryItem(request.ToContract());
        }

        public async Task MakeTemporaryItemPermanent(MakeTemporaryItemPermanentRequest request)
        {
            await client.MakeTemporaryItemPermanent(request.ToContract());
        }

        public async Task CreateStoreAsync(CreateStoreRequest request)
        {
            await client.CreateStore(request.ToContract());
        }

        public async Task ModifyStoreAsync(ModifyStoreRequest request)
        {
            await client.UpdateStore(request.ToContract());
        }
    }
}