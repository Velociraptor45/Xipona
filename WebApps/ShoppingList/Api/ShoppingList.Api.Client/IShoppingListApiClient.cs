using RestEase;
using ShoppingList.Api.Contracts.Commands.ChangeItem;
using ShoppingList.Api.Contracts.Commands.CreateItem;
using ShoppingList.Api.Contracts.Commands.CreateStore;
using ShoppingList.Api.Contracts.Commands.UpdateItem;
using ShoppingList.Api.Contracts.Commands.UpdateStore;
using ShoppingList.Api.Contracts.Queries;
using ShoppingList.Api.Contracts.Queries.AllActiveItemCategories;
using ShoppingList.Api.Contracts.Queries.AllActiveManufacturers;
using ShoppingList.Api.Contracts.Queries.AllActiveStores;
using ShoppingList.Api.Contracts.Queries.AllQuantityTypes;
using ShoppingList.Api.Contracts.Queries.ItemFilterResults;
using ShoppingList.Api.Contracts.SharedContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingList.Api.Client
{
    public interface IShoppingListApiClient
    {
        #region ShoppingListController

        [Post("shopping-list/{shoppingListId}/items/{itemId}/add/{quantity}")]
        Task AddItemToShoppingList([Path] int shoppingListId, [Path] int itemId, [Path] float quantity);

        [Post("shopping-list/{shoppingListId}/items/{itemId}/change-quantity/{quantity}")]
        Task ChangeItemQuantityOnShoppingList([Path] int shoppingListId, [Path] int itemId, [Path] float quantity);

        [Post("shopping-list/create/{storeId}")]
        Task CreatList([Path] int storeId);

        [Post("shopping-list/{shoppingListId}/finish")]
        Task FinishList([Path] int shoppingListId);

        [Get("shopping-list/active/{storeId}")]
        Task<ShoppingListContract> GetActiveShoppingListByStoreId([Path] int storeId);

        [Post("shopping-list/{shoppingListId}/items/{itemId}/put-in-basket")]
        Task PutItemInBasket([Path] int shoppingListId, [Path] int itemId);

        [Post("shopping-list/{shoppingListId}/items/{itemId}/remove-from-basket")]
        Task RemoveItemFromBasket([Path] int shoppingListId, [Path] int itemId);

        [Post("shopping-list/{shoppingListId}/items/{itemId}/remove")]
        Task RemoveItemFromShoppingList([Path] int shoppingListId, [Path] int itemId);

        [Get("shopping-list/quantity-types")]
        Task<IEnumerable<QuantityTypeContract>> GetAllQuantityTypes();

        [Get("shopping-list/quantity-in-packet-types")]
        Task<IEnumerable<QuantityInPacketTypeContract>> GetAllQuantityInPacketTypes();

        #endregion ShoppingListController

        #region ItemController

        [Post("item/create")]
        Task CreateItem([Body] CreateItemContract createItemContract);

        [Get("item/search/{searchInput}/{storeId}")]
        Task<IEnumerable<ItemSearchContract>> GetItemSearchResults([Path] string searchInput, [Path] int storeId);

        [Post("item/change")]
        Task ChangeItem([Body] ChangeItemContract changeItemContract);

        [Post("item/update")]
        Task UpdateItemAsync([Body] UpdateItemContract updateItemContract);

        [Get("item/filter")]
        Task<IEnumerable<ItemFilterResultContract>> GetItemFilterResult([Query] IEnumerable<int> storeIds,
            [Query] IEnumerable<int> itemCategoryIds, [Query] IEnumerable<int> manufacturerIds);

        #endregion ItemController

        #region StoreController

        [Get("store/active")]
        Task<IEnumerable<ActiveStoreContract>> GetAllActiveStores();

        [Post("store/create")]
        Task CreateStore([Body] CreateStoreContract createStoreContract);

        [Post("store/update")]
        Task UpdateStore([Body] UpdateStoreContract updateStoreContract);

        #endregion StoreController

        #region ManufacturerController

        [Get("manufacturer/search/{searchInput}")]
        Task<IEnumerable<ManufacturerContract>> GetManufacturerSearchResults([Path] string searchInput);

        [Get("manufacturer/all/active")]
        Task<IEnumerable<ActiveManufacturerContract>> GetAllActiveManufacturers();

        #endregion ManufacturerController

        #region ItemCategoryController

        [Get("item-category/search/{searchInput}")]
        Task<IEnumerable<ItemCategoryContract>> GetItemCategorySearchResults([Path] string searchInput);

        [Get("item-category/all/active")]
        Task<IEnumerable<ActiveItemCategoryContract>> GetAllActiveItemCategories();

        #endregion ItemCategoryController
    }
}