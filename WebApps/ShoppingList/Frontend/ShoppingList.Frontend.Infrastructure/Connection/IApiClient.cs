using ShoppingList.Frontend.Models;
using ShoppingList.Frontend.Models.Index.Search;
using ShoppingList.Frontend.Models.Items;
using ShoppingList.Frontend.Models.Shared.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingList.Frontend.Infrastructure.Connection
{
    public interface IApiClient
    {
        Task AddItemToShoppingListAsync(AddItemToShoppingListRequest request);
        Task ChangeItemAsync(ChangeItemRequest request);
        Task ChangeItemQuantityOnShoppingListAsync(ChangeItemQuantityOnShoppingListRequest request);
        Task CreateItemAsync(CreateItemRequest request);
        Task CreateItemCategoryAsync(string name);
        Task CreateManufacturerAsync(string name);
        Task DeleteItemAsync(DeleteItemRequest request);
        Task FinishListAsync(FinishListRequest request);
        Task<ShoppingListRoot> GetActiveShoppingListByStoreIdAsync(int storeId);
        Task<IEnumerable<ItemCategory>> GetAllActiveItemCategoriesAsync();
        Task<IEnumerable<Manufacturer>> GetAllActiveManufacturersAsync();
        Task<IEnumerable<Store>> GetAllActiveStoresAsync();
        Task<IEnumerable<QuantityInPacketType>> GetAllQuantityInPacketTypesAsync();
        Task<IEnumerable<QuantityType>> GetAllQuantityTypesAsync();
        Task<StoreItem> GetItemByIdAsync(int itemId);
        Task<IEnumerable<ItemFilterResult>> GetItemFilterResultAsync(IEnumerable<int> storeIds, IEnumerable<int> itemCategoryIds, IEnumerable<int> manufacturerIds);
        Task<IEnumerable<ItemSearchResult>> GetItemSearchResultsAsync(string searchInput, int storeId);
        Task IsAliveAsync();

        Task PutItemInBasketAsync(PutItemInBasketRequest request);

        Task RemoveItemFromBasketAsync(RemoveItemFromBasketRequest request);

        Task RemoveItemFromShoppingListAsync(RemoveItemFromShoppingListRequest request);
        Task UpdateItemAsync(UpdateItemRequest request);
    }
}