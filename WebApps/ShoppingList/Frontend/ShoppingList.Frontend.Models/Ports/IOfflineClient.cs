using ShoppingList.Frontend.Models.Index.Search;
using ShoppingList.Frontend.Models.Items;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingList.Frontend.Models.Ports
{
    public interface IOfflineClient
    {
        Task AddItemToShoppingListAsync(int shoppingListId, int itemId, float quantity);

        Task ChangeItemQuantityOnShoppingListAsync(int shoppingListId, int itemId, float quantity);

        Task FinishListAsync(int shoppingListId);

        Task<ShoppingListRoot> GetActiveShoppingListByStoreIdAsync(int storeId);

        Task<IEnumerable<ItemCategory>> GetAllActiveItemCategories();

        Task<IEnumerable<Manufacturer>> GetAllActiveManufacturers();

        Task<IEnumerable<Store>> GetAllActiveStoresAsync();
        Task<StoreItem> GetItemById(int itemId);
        Task<IEnumerable<ItemFilterResult>> GetItemFilterResult(IEnumerable<int> storeIds, IEnumerable<int> itemCategoryIds, IEnumerable<int> manufacturerIds);

        Task<IEnumerable<ItemSearchResult>> GetItemSearchResultsAsync(string searchInput, int storeId);

        Task PutItemInBasketAsync(int shoppingListId, int itemId);

        Task RemoveItemFromBasketAsync(int shoppingListId, int itemId);

        Task RemoveItemFromShoppingListAsync(int shoppingListId, int itemId);
    }
}