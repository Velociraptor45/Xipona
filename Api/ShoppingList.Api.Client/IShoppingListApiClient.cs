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
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.ItemFilterResults;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.ItemSearch;
using RestEase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Client
{
    public interface IShoppingListApiClient
    {
        #region ShoppingListController

        [Get("shopping-list/is-alive")]
        Task<bool> IsAlive();

        [Post("shopping-list/items/add")]
        Task AddItemToShoppingList([Body] AddItemToShoppingListContract contract);

        [Post("shopping-list/items/add-with-type")]
        Task AddItemWithTypeToShoppingList([Body] AddItemWithTypeToShoppingListContract contract);

        [Post("shopping-list/items/change-quantity")]
        Task ChangeItemQuantityOnShoppingList([Body] ChangeItemQuantityOnShoppingListContract contract);

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

        [Get("shopping-list/quantity-types-in-packet")]
        Task<IEnumerable<QuantityTypeInPacketContract>> GetAllQuantityTypesInPacket();

        #endregion ShoppingListController

        #region ItemController

        [Post("item/create")]
        Task CreateItem([Body] CreateItemContract createItemContract);

        [Get("item/search/{searchInput}/{storeId}")]
        Task<IEnumerable<ItemSearchContract>> GetItemSearchResults([Path] string searchInput, [Path] int storeId);

        [Post("item/modify")]
        Task ModifyItem([Body] ModifyItemContract modifyItemContract);

        [Post("item/modify-with-types")]
        Task ModifyItemWithTypesAsync([Body] ModifyItemWithTypesContract contract);

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

        [Post("item-category/delete")]
        Task DeleteItemCategory([Body] DeleteItemCategoryContract contract);

        #endregion ItemCategoryController
    }
}