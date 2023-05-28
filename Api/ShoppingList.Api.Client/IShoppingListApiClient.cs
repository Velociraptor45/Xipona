using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.ItemCategories.Commands;
using ProjectHermes.ShoppingList.Api.Contracts.ItemCategories.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.ModifyItem;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.UpdateItemPrice;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.UpdateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.SearchItemsForShoppingLists;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.Manufacturers.Commands;
using ProjectHermes.ShoppingList.Api.Contracts.Manufacturers.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.CreateRecipe;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.ModifyRecipe;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.ShoppingList.Api.Contracts.RecipeTags.Commands;
using ProjectHermes.ShoppingList.Api.Contracts.RecipeTags.Queries.GetAll;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Commands.ModifyStore;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresForShopping;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresOverview;
using RestEase;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AddItemToShoppingListContract = ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemToShoppingList.AddItemToShoppingListContract;

namespace ProjectHermes.ShoppingList.Api.Client
{
    public interface IShoppingListApiClient
    {
        [Get("monitoring/alive")]
        Task<bool> IsAlive();

        #region ShoppingListController

        [Get("shopping-lists/active/{storeId}")]
        Task<ShoppingListContract> GetActiveShoppingListByStoreIdAsync([Path] Guid storeId,
            CancellationToken cancellationToken = default);

        [Put("shopping-lists/{id}/items/temporary")]
        Task AddTemporaryItemToShoppingListAsync([Path] Guid id, [Body] AddTemporaryItemToShoppingListContract contract,
            CancellationToken cancellationToken = default);

        [Put("shopping-lists/{id}/items")]
        Task AddItemToShoppingListAsync([Path] Guid id, [Body] AddItemToShoppingListContract contract,
            CancellationToken cancellationToken = default);

        [Put("shopping-lists/{id}/items/{itemId}/{itemTypeId}")]
        Task AddItemWithTypeToShoppingListAsync([Path] Guid id, [Path] Guid itemId,
            [Path] Guid itemTypeId, [Body] AddItemWithTypeToShoppingListContract contract,
            CancellationToken cancellationToken = default);

        [Put("shopping-lists/add-items-to-shopping-lists")]
        Task AddItemsToShoppingListsAsync(
            [Body] AddItemsToShoppingListsContract contract, CancellationToken cancellationToken = default);

        [Put("shopping-lists/{id}/items/quantity")]
        Task ChangeItemQuantityOnShoppingListAsync([Path] Guid id,
            [Body] ChangeItemQuantityOnShoppingListContract contract, CancellationToken cancellationToken = default);

        [Put("shopping-lists/{id}/finish")]
        Task FinishListAsync([Path] Guid id, [Query(Format = "yyyy/MM/ddTHH:mm:ss")] DateTimeOffset? finishedAt,
            CancellationToken cancellationToken = default);

        [Put("shopping-lists/{id}/items/basket/add")]
        Task PutItemInBasketAsync([Path] Guid id, [Body] PutItemInBasketContract contract,
            CancellationToken cancellationToken = default);

        [Put("shopping-lists/{id}/items/basket/remove")]
        Task RemoveItemFromBasketAsync([Path] Guid id, [Body] RemoveItemFromBasketContract contract,
            CancellationToken cancellationToken = default);

        [Delete("shopping-lists/{id}/items")]
        Task RemoveItemFromShoppingListAsync([Path] Guid id,
            [Body] RemoveItemFromShoppingListContract contract, CancellationToken cancellationToken = default);

        #endregion ShoppingListController

        #region ItemController

        [Post("items/without-types")]
        Task CreateItemAsync([Body] CreateItemContract contract, CancellationToken cancellationToken = default);

        [Post("items/with-types")]
        Task CreateItemWithTypesAsync([Body] CreateItemWithTypesContract contract,
            CancellationToken cancellationToken = default);

        [Post("items/temporary")]
        Task CreateTemporaryItemAsync([Body] CreateTemporaryItemContract contract,
            CancellationToken cancellationToken = default);

        [Get("items/{id}")]
        Task<ItemContract> GetAsync([Path] Guid id, CancellationToken cancellationToken = default);

        [Get("items/search/{storeId}")]
        Task<IEnumerable<SearchItemForShoppingListResultContract>> SearchItemsForShoppingListAsync(
            [Path] Guid storeId, [Query] string searchInput, CancellationToken cancellationToken = default);

        [Get("items/search/by-item-category/{itemCategoryId}")]
        Task<IEnumerable<SearchItemByItemCategoryResultContract>> SearchItemsByItemCategoryAsync(
            [Path] Guid itemCategoryId, CancellationToken cancellationToken = default);

        [Get("items/search")]
        Task<IEnumerable<SearchItemResultContract>> SearchItemsAsync([Query] string searchInput,
            CancellationToken cancellationToken = default);

        [Get("items/filter")]
        Task<IEnumerable<SearchItemResultContract>> SearchItemsByFilterAsync([Query] IEnumerable<Guid> storeIds,
            [Query] IEnumerable<Guid> itemCategoryIds, [Query] IEnumerable<Guid> manufacturerIds,
            CancellationToken cancellationToken = default);

        [Get("items/quantity-types")]
        Task<IEnumerable<QuantityTypeContract>> GetAllQuantityTypesAsync(
            CancellationToken cancellationToken = default);

        [Get("items/quantity-types-in-packet")]
        Task<IEnumerable<QuantityTypeInPacketContract>> GetAllQuantityTypesInPacketAsync(
            CancellationToken cancellationToken = default);

        [Put("items/without-types/{id}/modify")]
        Task ModifyItemAsync([Path] Guid id, [Body] ModifyItemContract contract,
            CancellationToken cancellationToken = default);

        [Put("items/with-types/{id}/modify")]
        Task ModifyItemWithTypesAsync([Path] Guid id, [Body] ModifyItemWithTypesContract contract,
            CancellationToken cancellationToken = default);

        [Put("items/without-types/{id}/update")]
        Task UpdateItemAsync([Path] Guid id, [Body] UpdateItemContract contract,
            CancellationToken cancellationToken = default);

        [Put("items/{id}/update-price")]
        Task UpdateItemPriceAsync([Path] Guid id, [Body] UpdateItemPriceContract contract,
            CancellationToken cancellationToken = default);

        [Put("items/with-types/{id}/update")]
        Task UpdateItemWithTypesAsync([Path] Guid id, [Body] UpdateItemWithTypesContract contract,
            CancellationToken cancellationToken = default);

        [Put("items/temporary/{id}")]
        Task MakeTemporaryItemPermanentAsync([Path] Guid id, [Body] MakeTemporaryItemPermanentContract contract,
            CancellationToken cancellationToken = default);

        [Delete("items/{id}")]
        Task DeleteItemAsync([Path] Guid id, CancellationToken cancellationToken = default);

        #endregion ItemController

        #region StoreController

        [Get("stores/{id}")]
        Task<StoreContract> GetStoreByIdAsync([Path] Guid id, CancellationToken cancellationToken = default);

        [Get("stores/active-for-shopping")]
        Task<IEnumerable<StoreForShoppingContract>> GetActiveStoresForShoppingAsync(
            CancellationToken cancellationToken = default);

        [Get("stores/active-for-item")]
        Task<IEnumerable<StoreForItemContract>> GetActiveStoresForItemAsync(
            CancellationToken cancellationToken = default);

        [Get("stores/active-overview")]
        Task<IEnumerable<StoreSearchResultContract>> GetActiveStoresOverviewAsync(
            CancellationToken cancellationToken = default);

        [Post("stores")]
        Task<StoreContract> CreateStoreAsync([Body] CreateStoreContract createStoreContract,
            CancellationToken cancellationToken = default);

        [Put("stores")]
        Task ModifyStoreAsync([Body] ModifyStoreContract modifyStoreContract,
            CancellationToken cancellationToken = default);

        #endregion StoreController

        #region ManufacturerController

        [Get("manufacturers/{id}")]
        Task<ManufacturerContract> GetManufacturerByIdAsync([Path] Guid id,
            CancellationToken cancellationToken = default);

        [Get("manufacturers")]
        Task<IEnumerable<ManufacturerSearchResultContract>> GetManufacturerSearchResultsAsync(
            [Query] string searchInput, [Query] bool includeDeleted, CancellationToken cancellationToken = default);

        [Get("manufacturers/active")]
        Task<IEnumerable<ManufacturerContract>> GetAllActiveManufacturersAsync(
            CancellationToken cancellationToken = default);

        [Put("manufacturers")]
        Task ModifyManufacturerAsync([Body] ModifyManufacturerContract contract,
            CancellationToken cancellationToken = default);

        [Post("manufacturers")]
        Task<ManufacturerContract> CreateManufacturerAsync([Query] string name,
            CancellationToken cancellationToken = default);

        [Delete("manufacturers/{id}")]
        Task DeleteManufacturerAsync([Path] Guid id, CancellationToken cancellationToken = default);

        #endregion ManufacturerController

        #region ItemCategoryController

        [Get("item-categories/{id}")]
        Task<ItemCategoryContract> GetItemCategoryByIdAsync([Path] Guid id,
            CancellationToken cancellationToken = default);

        [Get("item-categories")]
        Task<IEnumerable<ItemCategorySearchResultContract>> SearchItemCategoriesByNameAsync([Query] string searchInput,
            [Query] bool includeDeleted, CancellationToken cancellationToken = default);

        [Get("item-categories/active")]
        Task<IEnumerable<ItemCategoryContract>> GetAllActiveItemCategoriesAsync(
            CancellationToken cancellationToken = default);

        [Put("item-categories")]
        Task ModifyItemCategoryAsync([Body] ModifyItemCategoryContract contract,
            CancellationToken cancellationToken = default);

        [Post("item-categories")]
        Task<ItemCategoryContract> CreateItemCategoryAsync([Query] string name,
            CancellationToken cancellationToken = default);

        [Delete("item-categories/{id}")]
        Task DeleteItemCategoryAsync([Path] Guid id, CancellationToken cancellationToken = default);

        #endregion ItemCategoryController

        #region RecipeController

        [Get("recipes/{id}")]
        Task<RecipeContract> GetRecipeByIdAsync([Path] Guid id, CancellationToken cancellationToken = default);

        [Get("recipes/search-by-name")]
        Task<IEnumerable<RecipeSearchResultContract>> SearchRecipesByNameAsync([Query] string searchInput,
            CancellationToken cancellationToken = default);

        [Get("recipes/search-by-tags")]
        Task<IEnumerable<RecipeSearchResultContract>> SearchRecipesByTagsAsync([Query] IEnumerable<Guid> tagIds,
            CancellationToken cancellationToken = default);

        [Get("recipes/ingredient-quantity-types")]
        Task<IEnumerable<IngredientQuantityTypeContract>> GetAllIngredientQuantityTypes(
            CancellationToken cancellationToken = default);

        [Get("recipes/{id}/item-amounts-for-one-serving")]
        Task<ItemAmountsForOneServingContract> GetItemAmountsForOneServingAsync([Path] Guid id,
            CancellationToken cancellationToken = default);

        [Post("recipes")]
        Task<RecipeContract> CreateRecipeAsync([Body] CreateRecipeContract contract,
            CancellationToken cancellationToken = default);

        [Put("recipes/{id}/modify")]
        Task ModifyRecipeAsync([Path] Guid id, [Body] ModifyRecipeContract contract,
            CancellationToken cancellationToken = default);

        #endregion RecipeController

        #region RecipeTagController

        [Get("recipe-tags/all")]
        Task<IEnumerable<RecipeTagContract>> GetAllRecipeTagsAsync(CancellationToken cancellationToken = default);

        [Post("recipe-tags")]
        Task<RecipeTagContract> CreateRecipeTagAsync([Body] CreateRecipeTagContract contract,
            CancellationToken cancellationToken = default);

        #endregion RecipeTagController
    }
}