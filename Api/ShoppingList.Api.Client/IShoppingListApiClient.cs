using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
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
        Task FinishList([Path] Guid shoppingListId);

        [Get("shopping-list/active/{storeId}")]
        Task<ShoppingListContract> GetActiveShoppingListByStoreId([Path] Guid storeId);

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

        [Post("items/without-types")]
        Task CreateItemAsync([Body] CreateItemContract createItemContract);

        [Post("items/with-types")]
        Task CreateItemWithTypesAsync([Body] CreateItemWithTypesContract createItemWithTypesContract);

        [Get("items/search-for-shopping-list/{searchInput}/{storeId}")]
        Task<IEnumerable<SearchItemForShoppingListResultContract>> SearchItemsForShoppingListAsync([Path] string searchInput,
            [Path] Guid storeId);

        [Get("items/search/{searchInput}")]
        Task<IEnumerable<SearchItemResultContract>> SearchItemsAsync([Path] string searchInput);

        [Get("items/search-by-filter")]
        Task<IEnumerable<SearchItemResultContract>> SearchItemsByFilterAsync([Query] IEnumerable<Guid> storeIds,
            [Query] IEnumerable<Guid> itemCategoryIds, [Query] IEnumerable<Guid> manufacturerIds);

        [Put("items/without-types/{id}/modify")]
        Task ModifyItemAsync([Path] Guid id, [Body] ModifyItemContract contract);

        [Put("items/with-types/{id}/modify")]
        Task ModifyItemWithTypesAsync([Path] Guid id, [Body] ModifyItemWithTypesContract contract);

        [Put("items/without-types/{id}/update")]
        Task UpdateItemAsync([Path] Guid id, [Body] UpdateItemContract contract);

        [Put("items/with-types/{id}/update")]
        Task UpdateItemWithTypesAsync([Path] Guid id, [Body] UpdateItemWithTypesContract contract);

        [Post("items/delete/{itemId}")]
        Task DeleteItemAsync([Path] Guid itemId);

        [Get("items/{itemId}")]
        Task<StoreItemContract> Get([Path] Guid itemId);

        [Post("items/create/temporary")]
        Task CreateTemporaryItem([Body] CreateTemporaryItemContract contract);

        [Post("items/make-temporary-item-permanent")]
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
        Task<IEnumerable<ManufacturerContract>> GetAllActiveManufacturers();

        [Post("manufacturer/create/{name}")]
        Task CreateManufacturer([Path] string name);

        #endregion ManufacturerController

        #region ItemCategoryController

        [Get("item-categories")]
        Task<IEnumerable<ItemCategoryContract>> SearchItemCategoriesByName([Query] string searchInput);

        [Get("item-categories/active")]
        Task<IEnumerable<ItemCategoryContract>> GetAllActiveItemCategories();

        [Post("item-categories")]
        Task<ItemCategoryContract> CreateItemCategory([Query] string name);

        [Delete("item-categories/{id}")]
        Task DeleteItemCategory([Path] Guid id);

        #endregion ItemCategoryController
    }
}