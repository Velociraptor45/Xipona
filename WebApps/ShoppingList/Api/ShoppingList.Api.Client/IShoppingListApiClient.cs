using RestEase;
using ShoppingList.Api.Contracts.Commands.AddItemToShoppingList;
using ShoppingList.Api.Contracts.Commands.ChangeItem;
using ShoppingList.Api.Contracts.Commands.ChangeItemQuantityOnShoppingList;
using ShoppingList.Api.Contracts.Commands.CreateItem;
using ShoppingList.Api.Contracts.Commands.CreateStore;
using ShoppingList.Api.Contracts.Commands.CreateTemporaryItem;
using ShoppingList.Api.Contracts.Commands.MakeTemporaryItemPermanent;
using ShoppingList.Api.Contracts.Commands.PutItemInBasket;
using ShoppingList.Api.Contracts.Commands.RemoveItemFromBasket;
using ShoppingList.Api.Contracts.Commands.RemoveItemFromShoppingList;
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

        [Get("shopping-list/is-alive")]
        Task<bool> IsAlive();

        [Post("shopping-list/items/add")]
        Task AddItemToShoppingList([Body] AddItemToShoppingListContract contract);

        [Post("shopping-list/items/change-quantity")]
        Task ChangeItemQuantityOnShoppingList([Body] ChangeItemQuantityOnShoppingListContract contract);

        [Post("shopping-list/create/{storeId}")]
        Task CreatList([Path] int storeId);

        [Post("shopping-list/{shoppingListId}/finish")]
        Task FinishList([Path] int shoppingListId);

        [Get("shopping-list/active/{storeId}")]
        Task<ShoppingListContract> GetActiveShoppingListByStoreId([Path] int storeId);

        [Post("shopping-list/items/put-in-basket")]
        Task PutItemInBasket([Body] PutItemInBasketContract contract);

        [Post("shopping-list/items/remove-from-basket")]
        Task RemoveItemFromBasket([Body] RemoveItemFromBasketContract contract);

        [Post("shopping-list/items/remove")]
        Task RemoveItemFromShoppingList([Body] RemoveItemFromShoppingListContract contract);

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

        [Post("item/delete/{itemId}")]
        Task DeleteItemAsync([Path] int itemId);

        [Get("item/filter")]
        Task<IEnumerable<ItemFilterResultContract>> GetItemFilterResult([Query] IEnumerable<int> storeIds,
            [Query] IEnumerable<int> itemCategoryIds, [Query] IEnumerable<int> manufacturerIds);

        [Get("item/{itemId}")]
        Task<StoreItemContract> Get([Path] int itemId);

        [Post("item/create/temporary")]
        Task CreateTemporaryItem([Body] CreateTemporaryItemContract contract);

        [Post("item/make-temporary-item-permanent")]
        Task MakeTemporaryItemPermanent([Body] MakeTemporaryItemPermanentContract contract);

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

        [Post("manufacturer/create/{name}")]
        Task CreateManufacturer([Path] string name);

        #endregion ManufacturerController

        #region ItemCategoryController

        [Get("item-category/search/{searchInput}")]
        Task<IEnumerable<ItemCategoryContract>> GetItemCategorySearchResults([Path] string searchInput);

        [Get("item-category/all/active")]
        Task<IEnumerable<ActiveItemCategoryContract>> GetAllActiveItemCategories();

        [Post("item-category/create/{name}")]
        Task CreateItemCategory([Path] string name);

        #endregion ItemCategoryController
    }
}