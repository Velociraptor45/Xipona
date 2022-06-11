using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.ItemCategories.Commands;
using ProjectHermes.ShoppingList.Api.Contracts.Manufacturers.Commands;
using ProjectHermes.ShoppingList.Api.Contracts.Manufacturers.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.AllQuantityTypes;
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
        [Get("monitoring/alive")]
        Task<bool> IsAlive();

        #region ShoppingListController

        [Get("shopping-lists/active/{storeId}")]
        Task<ShoppingListContract> GetActiveShoppingListByStoreIdAsync([Path] Guid storeId);

        [Put("shopping-lists/{id}/items")]
        Task AddItemToShoppingListAsync([Path] Guid id, [Body] AddItemToShoppingListContract contract);

        [Put("shopping-lists/{id}/items/{itemId}/{itemTypeId}")]
        Task AddItemWithTypeToShoppingListAsync([Path] Guid id, [Path] Guid itemId,
            [Path] Guid itemTypeId, [Body] AddItemWithTypeToShoppingListContract contract);

        [Put("shopping-lists/{id}/items/quantity")]
        Task ChangeItemQuantityOnShoppingListAsync([Path] Guid id,
            [Body] ChangeItemQuantityOnShoppingListContract contract);

        [Put("shopping-lists/{id}/finish")]
        Task FinishListAsync([Path] Guid id);

        [Put("shopping-lists/{id}/items/basket/add")]
        Task PutItemInBasketAsync([Path] Guid id, [Body] PutItemInBasketContract contract);

        [Put("shopping-lists/{id}/items/basket/remove")]
        Task RemoveItemFromBasketAsync([Path] Guid id, [Body] RemoveItemFromBasketContract contract);

        [Delete("shopping-lists/{id}/items")]
        Task RemoveItemFromShoppingListAsync([Path] Guid id,
            [Body] RemoveItemFromShoppingListContract contract);

        #endregion ShoppingListController

        #region ItemController

        [Post("items/without-types")]
        Task CreateItemAsync([Body] CreateItemContract contract);

        [Post("items/with-types")]
        Task CreateItemWithTypesAsync([Body] CreateItemWithTypesContract contract);

        [Post("items/temporary")]
        Task CreateTemporaryItemAsync([Body] CreateTemporaryItemContract contract);

        [Get("items/{id}")]
        Task<StoreItemContract> GetAsync([Path] Guid id);

        [Get("items/search/{storeId}")]
        Task<IEnumerable<SearchItemForShoppingListResultContract>> SearchItemsForShoppingListAsync(
            [Path] Guid storeId, [Query] string searchInput);

        [Get("items/search")]
        Task<IEnumerable<SearchItemResultContract>> SearchItemsAsync([Query] string searchInput);

        [Get("items/filter")]
        Task<IEnumerable<SearchItemResultContract>> SearchItemsByFilterAsync([Query] IEnumerable<Guid> storeIds,
            [Query] IEnumerable<Guid> itemCategoryIds, [Query] IEnumerable<Guid> manufacturerIds);

        [Get("items/quantity-types")]
        Task<IEnumerable<QuantityTypeContract>> GetAllQuantityTypesAsync();

        [Get("items/quantity-types-in-packet")]
        Task<IEnumerable<QuantityTypeInPacketContract>> GetAllQuantityTypesInPacketAsync();

        [Put("items/without-types/{id}/modify")]
        Task ModifyItemAsync([Path] Guid id, [Body] ModifyItemContract contract);

        [Put("items/with-types/{id}/modify")]
        Task ModifyItemWithTypesAsync([Path] Guid id, [Body] ModifyItemWithTypesContract contract);

        [Put("items/without-types/{id}/update")]
        Task UpdateItemAsync([Path] Guid id, [Body] UpdateItemContract contract);

        [Put("items/with-types/{id}/update")]
        Task UpdateItemWithTypesAsync([Path] Guid id, [Body] UpdateItemWithTypesContract contract);

        [Put("items/temporary/{id}")]
        Task MakeTemporaryItemPermanentAsync([Path] Guid id, [Body] MakeTemporaryItemPermanentContract contract);

        [Delete("items/{id}")]
        Task DeleteItemAsync([Path] Guid id);

        #endregion ItemController

        #region StoreController

        [Get("stores/active")]
        Task<IEnumerable<ActiveStoreContract>> GetAllActiveStoresAsync();

        [Post("stores")]
        Task<StoreContract> CreateStoreAsync([Body] CreateStoreContract createStoreContract);

        [Put("stores")]
        Task UpdateStoreAsync([Body] UpdateStoreContract updateStoreContract);

        #endregion StoreController

        #region ManufacturerController

        [Get("manufacturers/{id}")]
        Task<ManufacturerContract> GetManufacturerByIdAsync([Path] Guid id);

        [Get("manufacturers")]
        Task<IEnumerable<ManufacturerSearchResultContract>> GetManufacturerSearchResultsAsync(
            [Query] string searchInput, [Query] bool includeDeleted);

        [Get("manufacturers/active")]
        Task<IEnumerable<ManufacturerContract>> GetAllActiveManufacturersAsync();

        [Put("manufacturers")]
        Task ModifyManufacturerAsync([Body] ModifyManufacturerContract contract);

        [Post("manufacturers")]
        Task<ManufacturerContract> CreateManufacturerAsync([Query] string name);

        [Delete("manufacturers/{id}")]
        Task DeleteManufacturerAsync([Path] Guid id);

        #endregion ManufacturerController

        #region ItemCategoryController

        [Get("item-categories")]
        Task<IEnumerable<ItemCategoryContract>> SearchItemCategoriesByName([Query] string searchInput);

        [Get("item-categories/active")]
        Task<IEnumerable<ItemCategoryContract>> GetAllActiveItemCategories();

        [Put("item-categories")]
        Task ModifyItemCategoryAsync([Body] ModifyItemCategoryContract contract);

        [Post("item-categories")]
        Task<ItemCategoryContract> CreateItemCategory([Query] string name);

        [Delete("item-categories/{id}")]
        Task DeleteItemCategory([Path] Guid id);

        #endregion ItemCategoryController
    }
}