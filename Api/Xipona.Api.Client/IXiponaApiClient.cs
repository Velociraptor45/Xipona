using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Contracts.ItemCategories.Commands;
using ProjectHermes.Xipona.Api.Contracts.ItemCategories.Queries;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.CreateItem;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.CreateItemWithTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.ModifyItem;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItem;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItemPrice;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItemWithTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.SearchItemsForShoppingLists;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Shared;
using ProjectHermes.Xipona.Api.Contracts.Manufacturers.Commands;
using ProjectHermes.Xipona.Api.Contracts.Manufacturers.Queries;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Commands.CreateRecipe;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Commands.ModifyRecipe;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Commands;
using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Queries.GetAll;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.PutItemInBasket;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromBasket;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.Xipona.Api.Contracts.Stores.Commands.CreateStore;
using ProjectHermes.Xipona.Api.Contracts.Stores.Commands.ModifyStore;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Get;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForShopping;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresOverview;
using RestEase;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AddItemToShoppingListContract = ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemToShoppingList.AddItemToShoppingListContract;

namespace ProjectHermes.Xipona.Api.Client
{
    /// <summary>
    /// Client for the Xipona API.
    /// </summary>
    public interface IShoppingListApiClient
    {
        /// <summary>
        /// Used to check if the API is reachable. Always returns true.
        /// </summary>
        /// <returns></returns>
        [Get("monitoring/alive")]
        Task<bool> IsAlive();

        #region ShoppingListController

        /// <summary>
        /// Gets the active shopping list for the given store.
        /// </summary>
        /// <param name="storeId">The store's ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("shopping-lists/active/{storeId}")]
        Task<ShoppingListContract> GetActiveShoppingListByStoreIdAsync([Path] Guid storeId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a temporary item to the shopping list.
        /// </summary>
        /// <param name="id">The shopping list's ID</param>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Put("shopping-lists/{id}/items/temporary")]
        Task<TemporaryShoppingListItemContract> AddTemporaryItemToShoppingListAsync([Path] Guid id,
            [Body] AddTemporaryItemToShoppingListContract contract, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an existing item to the shopping list.
        /// </summary>
        /// <param name="id">The shopping list's ID</param>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Put("shopping-lists/{id}/items")]
        Task AddItemToShoppingListAsync([Path] Guid id, [Body] AddItemToShoppingListContract contract,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an existing item type to the shopping list.
        /// </summary>
        /// <param name="id">The shopping list's ID</param>
        /// <param name="itemId">The item's ID</param>
        /// <param name="itemTypeId">The item type's ID</param>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Put("shopping-lists/{id}/items/{itemId}/{itemTypeId}")]
        Task AddItemWithTypeToShoppingListAsync([Path] Guid id, [Path] Guid itemId,
            [Path] Guid itemTypeId, [Body] AddItemWithTypeToShoppingListContract contract,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds multiple items to multiple shopping lists.
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Put("shopping-lists/add-items-to-shopping-lists")]
        Task AddItemsToShoppingListsAsync(
            [Body] AddItemsToShoppingListsContract contract, CancellationToken cancellationToken = default);

        /// <summary>
        /// Changes the quantity of an item on the shopping list.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Put("shopping-lists/{id}/items/quantity")]
        Task ChangeItemQuantityOnShoppingListAsync([Path] Guid id,
            [Body] ChangeItemQuantityOnShoppingListContract contract, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finishes the shopping list. This will set the <paramref name="finishedAt"/> as the completion date
        /// and create a new shopping list for this store. All items not in the basket will be added to the new shopping list.
        /// </summary>
        /// <param name="id">The shopping list's ID</param>
        /// <param name="finishedAt">The date and time when shopping was finished</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Put("shopping-lists/{id}/finish")]
        Task FinishListAsync([Path] Guid id, [Query(Format = "yyyy/MM/ddTHH:mm:ss")] DateTimeOffset? finishedAt,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Puts an item in a shopping list's basket.
        /// </summary>
        /// <param name="id">The shopping list's ID</param>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Put("shopping-lists/{id}/items/basket/add")]
        Task PutItemInBasketAsync([Path] Guid id, [Body] PutItemInBasketContract contract,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an item from a shopping list's basket.
        /// </summary>
        /// <param name="id">The shopping list's ID</param>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Put("shopping-lists/{id}/items/basket/remove")]
        Task RemoveItemFromBasketAsync([Path] Guid id, [Body] RemoveItemFromBasketContract contract,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an item from a shopping list.
        /// </summary>
        /// <param name="id">The shopping list's ID</param>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Delete("shopping-lists/{id}/items")]
        Task RemoveItemFromShoppingListAsync([Path] Guid id,
            [Body] RemoveItemFromShoppingListContract contract, CancellationToken cancellationToken = default);

        #endregion ShoppingListController

        #region ItemController

        /// <summary>
        /// Creates an item.
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Post("items/without-types")]
        Task CreateItemAsync([Body] CreateItemContract contract, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates an item with types.
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Post("items/with-types")]
        Task CreateItemWithTypesAsync([Body] CreateItemWithTypesContract contract,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets an item by its ID. Does not include temporary items.
        /// </summary>
        /// <param name="id">The item's ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("items/{id}")]
        Task<ItemContract> GetAsync([Path] Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches for items for a shopping list by name or item category name. Does not include temporary items.
        /// </summary>
        /// <param name="storeId">The store at which the item must be available</param>
        /// <param name="searchInput">The search term</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("items/search/{storeId}")]
        Task<IEnumerable<SearchItemForShoppingListResultContract>> SearchItemsForShoppingListAsync(
            [Path] Guid storeId, [Query] string searchInput, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all items that belong to the given item category.
        /// </summary>
        /// <param name="itemCategoryId">The item category's ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("items/search/by-item-category/{itemCategoryId}")]
        Task<IEnumerable<SearchItemByItemCategoryResultContract>> SearchItemsByItemCategoryAsync(
            [Path] Guid itemCategoryId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the total number of item search results for the given search input.
        /// The actual search results can be retrieved with <see cref="SearchItemsAsync"/>.
        /// </summary>
        /// <param name="searchInput">The search term</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("items/search-result-count")]
        Task<int> GetTotalSearchResultCountAsync([Query] string searchInput,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches for items by name, but not by item type name. Does not include temporary items.
        /// </summary>
        /// <param name="searchInput">The search term</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("items/search")]
        Task<IEnumerable<SearchItemResultContract>> SearchItemsAsync([Query] string searchInput, [Query] int page = 1,
            [Query] int pageSize = 20, CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches for items by filter.
        /// Different filter criteria are connected via AND. Elements inside a filter criteria are connected via OR.
        /// Does not include temporary items.
        /// </summary>
        /// <param name="storeIds"></param>
        /// <param name="itemCategoryIds"></param>
        /// <param name="manufacturerIds"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("items/filter")]
        Task<IEnumerable<SearchItemResultContract>> SearchItemsByFilterAsync([Query] IEnumerable<Guid> storeIds,
            [Query] IEnumerable<Guid> itemCategoryIds, [Query] IEnumerable<Guid> manufacturerIds,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all quantity types.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("items/quantity-types")]
        Task<IEnumerable<QuantityTypeContract>> GetAllQuantityTypesAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all in packet quantity types.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("items/quantity-types-in-packet")]
        Task<IEnumerable<QuantityTypeInPacketContract>> GetAllQuantityTypesInPacketAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Modifies an existing item.
        /// </summary>
        /// <param name="id">The item's ID</param>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Put("items/without-types/{id}/modify")]
        Task ModifyItemAsync([Path] Guid id, [Body] ModifyItemContract contract,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Modifies an existing item with types.
        /// </summary>
        /// <param name="id">The item's ID</param>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Put("items/with-types/{id}/modify")]
        Task ModifyItemWithTypesAsync([Path] Guid id, [Body] ModifyItemWithTypesContract contract,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing item.
        /// </summary>
        /// <param name="id">The item's ID</param>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Put("items/without-types/{id}/update")]
        Task UpdateItemAsync([Path] Guid id, [Body] UpdateItemContract contract,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the price of an existing item.
        /// </summary>
        /// <param name="id">The item's ID</param>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Put("items/{id}/update-price")]
        Task UpdateItemPriceAsync([Path] Guid id, [Body] UpdateItemPriceContract contract,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing item with types.
        /// </summary>
        /// <param name="id">The item's ID</param>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Put("items/with-types/{id}/update")]
        Task UpdateItemWithTypesAsync([Path] Guid id, [Body] UpdateItemWithTypesContract contract,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Turns a temporary item into a normal, permanent item.
        /// </summary>
        /// <param name="id">The temporary item's ID</param>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Put("items/temporary/{id}")]
        Task MakeTemporaryItemPermanentAsync([Path] Guid id, [Body] MakeTemporaryItemPermanentContract contract,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Soft-deletes an item. The item will be removed from all active shopping lists (not the finished ones)
        /// and from all recipes where it's the default item for a recipe.
        /// </summary>
        /// <param name="id">The item's ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Delete("items/{id}")]
        Task DeleteItemAsync([Path] Guid id, CancellationToken cancellationToken = default);

        #endregion ItemController

        #region StoreController

        /// <summary>
        /// Gets a store by its ID.
        /// </summary>
        /// <param name="id">The store's ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("stores/{id}")]
        Task<StoreContract> GetStoreByIdAsync([Path] Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the full model of all active stores.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("stores/active-for-shopping")]
        Task<IEnumerable<StoreForShoppingContract>> GetActiveStoresForShoppingAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all active stores where an item is available.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("stores/active-for-item")]
        Task<IEnumerable<StoreForItemContract>> GetActiveStoresForItemAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a reduced model of all active stores.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("stores/active-overview")]
        Task<IEnumerable<StoreSearchResultContract>> GetActiveStoresOverviewAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a store.
        /// </summary>
        /// <param name="createStoreContract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Post("stores")]
        Task<StoreContract> CreateStoreAsync([Body] CreateStoreContract createStoreContract,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Modifies an existing store.
        /// </summary>
        /// <param name="modifyStoreContract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Put("stores")]
        Task ModifyStoreAsync([Body] ModifyStoreContract modifyStoreContract,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Soft-deletes a store. The active shopping list for this store will be hard-deleted.
        /// All items that are available in this stole will lose their availability for this store.
        /// If they are only available in this store, they will be soft-deleted as well.
        /// </summary>
        /// <param name="id">The store's ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Delete("stores/{id}")]
        Task DeleteStoreAsync([Path] Guid id, CancellationToken cancellationToken = default);

        #endregion StoreController

        #region ManufacturerController

        /// <summary>
        /// Gets a manufacturer by its ID.
        /// </summary>
        /// <param name="id">The manufacturer's ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("manufacturers/{id}")]
        Task<ManufacturerContract> GetManufacturerByIdAsync([Path] Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches for manufacturers by name.
        /// </summary>
        /// <param name="searchInput">The search term</param>
        /// <param name="includeDeleted">Whether to include deleted manufacturers or not</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("manufacturers")]
        Task<IEnumerable<ManufacturerSearchResultContract>> GetManufacturerSearchResultsAsync(
            [Query] string searchInput, [Query] bool includeDeleted, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all active manufacturers.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("manufacturers/active")]
        Task<IEnumerable<ManufacturerContract>> GetAllActiveManufacturersAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Modifies an existing manufacturer.
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Put("manufacturers")]
        Task ModifyManufacturerAsync([Body] ModifyManufacturerContract contract,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a manufacturer.
        /// </summary>
        /// <param name="name">The manufacturer's name</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Post("manufacturers")]
        Task<ManufacturerContract> CreateManufacturerAsync([Query] string name,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a manufacturer. All items of this manufacturer will lose their manufacturer.
        /// </summary>
        /// <param name="id">The manufacturer's ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Delete("manufacturers/{id}")]
        Task DeleteManufacturerAsync([Path] Guid id, CancellationToken cancellationToken = default);

        #endregion ManufacturerController

        #region ItemCategoryController

        /// <summary>
        /// Gets an item category by its ID.
        /// </summary>
        /// <param name="id">The item category's ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("item-categories/{id}")]
        Task<ItemCategoryContract> GetItemCategoryByIdAsync([Path] Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches for item categories by name.
        /// </summary>
        /// <param name="searchInput">The search term</param>
        /// <param name="includeDeleted">Whether to include deleted item categories or not</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("item-categories")]
        Task<IEnumerable<ItemCategorySearchResultContract>> SearchItemCategoriesByNameAsync([Query] string searchInput,
            [Query] bool includeDeleted, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all active item categories.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("item-categories/active")]
        Task<IEnumerable<ItemCategoryContract>> GetAllActiveItemCategoriesAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Modifies an existing item category.
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Put("item-categories")]
        Task ModifyItemCategoryAsync([Body] ModifyItemCategoryContract contract,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates an item category.
        /// </summary>
        /// <param name="name">The item category's name</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Post("item-categories")]
        Task<ItemCategoryContract> CreateItemCategoryAsync([Query] string name,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Soft-deletes an item category. All items in this category will be soft-deleted as well.
        /// </summary>
        /// <param name="id">The item category's ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Delete("item-categories/{id}")]
        Task DeleteItemCategoryAsync([Path] Guid id, CancellationToken cancellationToken = default);

        #endregion ItemCategoryController

        #region RecipeController

        /// <summary>
        /// Gets a recipe by its ID.
        /// </summary>
        /// <param name="id">The recipe's ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("recipes/{id}")]
        Task<RecipeContract> GetRecipeByIdAsync([Path] Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches for recipes by name.
        /// </summary>
        /// <param name="searchInput">The search term</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("recipes/search-by-name")]
        Task<IEnumerable<RecipeSearchResultContract>> SearchRecipesByNameAsync([Query] string searchInput,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches for recipes by tags, connected via AND.
        /// </summary>
        /// <param name="tagIds">The tags' IDs</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("recipes/search-by-tags")]
        Task<IEnumerable<RecipeSearchResultContract>> SearchRecipesByTagsAsync([Query] IEnumerable<Guid> tagIds,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all ingredient quantity types.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("recipes/ingredient-quantity-types")]
        Task<IEnumerable<IngredientQuantityTypeContract>> GetAllIngredientQuantityTypes(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the normalized item amounts for one serving of a recipe.
        /// </summary>
        /// <param name="id">The recipe's ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("recipes/{id}/item-amounts-for-one-serving")]
        Task<ItemAmountsForOneServingContract> GetItemAmountsForOneServingAsync([Path] Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a recipe.
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Post("recipes")]
        Task<RecipeContract> CreateRecipeAsync([Body] CreateRecipeContract contract,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Modifies an existing recipe.
        /// </summary>
        /// <param name="id">The recipe's ID</param>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Put("recipes/{id}/modify")]
        Task ModifyRecipeAsync([Path] Guid id, [Body] ModifyRecipeContract contract,
            CancellationToken cancellationToken = default);

        #endregion RecipeController

        #region RecipeTagController

        /// <summary>
        /// Gets all recipe tags.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Get("recipe-tags/all")]
        Task<IEnumerable<RecipeTagContract>> GetAllRecipeTagsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a recipe tag.
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Post("recipe-tags")]
        Task<RecipeTagContract> CreateRecipeTagAsync([Body] CreateRecipeTagContract contract,
            CancellationToken cancellationToken = default);

        #endregion RecipeTagController
    }
}