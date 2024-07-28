using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using ProjectHermes.Xipona.Frontend.Redux.Manufacturers.States;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ItemCategories;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.Items;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.Manufacturers;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.Stores.States;
using IngredientQuantityType = ProjectHermes.Xipona.Frontend.Redux.Recipes.States.IngredientQuantityType;
using ItemStore = ProjectHermes.Xipona.Frontend.Redux.Items.States.ItemStore;
using ShoppingListStore = ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.ShoppingListStore;

namespace ProjectHermes.Xipona.Frontend.Redux.Shared.Ports;

public interface IApiClient
{
    Task AddItemToShoppingListAsync(AddItemToShoppingListRequest request);

    Task ModifyItemAsync(EditedItem item);

    Task ChangeItemQuantityOnShoppingListAsync(ChangeItemQuantityOnShoppingListRequest request);

    Task CreateItemAsync(EditedItem item);

    Task<EditedItemCategory> CreateItemCategoryAsync(string name);

    Task<EditedManufacturer> CreateManufacturerAsync(string name);

    Task DeleteItemAsync(Guid itemId);

    Task FinishListAsync(FinishListRequest request);

    Task<IEnumerable<QuantityTypeInPacket>> GetAllQuantityTypesInPacketAsync();

    Task<IEnumerable<QuantityType>> GetAllQuantityTypesAsync();

    Task<EditedItem> GetItemByIdAsync(Guid itemId);

    Task<int> GetTotalSearchResultCountAsync(string searchInput);

    Task<IEnumerable<ItemSearchResult>> SearchItemsAsync(string searchInput,
        int page, int pageSize);

    Task<IEnumerable<SearchItemForShoppingListResult>> SearchItemsForShoppingListAsync(string searchInput,
        Guid storeId, CancellationToken cancellationToken);

    Task IsAliveAsync();

    Task MakeTemporaryItemPermanent(MakeTemporaryItemPermanentRequest request);

    Task PutItemInBasketAsync(PutItemInBasketRequest request);

    Task RemoveItemFromBasketAsync(RemoveItemFromBasketRequest request);

    Task RemoveItemFromShoppingListAsync(RemoveItemFromShoppingListRequest request);

    Task UpdateItemAsync(EditedItem item);

    Task CreateStoreAsync(EditedStore store);

    Task ModifyStoreAsync(EditedStore store);

    Task ModifyItemWithTypesAsync(EditedItem item);

    Task AddItemWithTypeToShoppingListAsync(AddItemWithTypeToShoppingListRequest request);

    Task UpdateItemWithTypesAsync(EditedItem item);

    Task CreateItemWithTypesAsync(EditedItem item);

    Task<IEnumerable<ManufacturerSearchResult>> GetManufacturerSearchResultsAsync(string searchInput);

    Task<EditedManufacturer> GetManufacturerByIdAsync(Guid id);

    Task DeleteManufacturerAsync(Guid id);

    Task ModifyManufacturerAsync(ModifyManufacturerRequest request);

    Task<EditedItemCategory> GetItemCategoryByIdAsync(Guid id);

    Task<IEnumerable<ItemCategorySearchResult>> GetItemCategorySearchResultsAsync(string searchInput);

    Task DeleteItemCategoryAsync(Guid id);

    Task ModifyItemCategoryAsync(ModifyItemCategoryRequest request);

    Task UpdateItemPriceAsync(UpdateItemPriceRequest request);

    Task<EditedRecipe> GetRecipeByIdAsync(Guid recipeId);

    Task<IEnumerable<RecipeSearchResult>> SearchRecipesByNameAsync(string searchInput);

    Task<EditedRecipe> CreateRecipeAsync(EditedRecipe recipe);

    Task ModifyRecipeAsync(EditedRecipe recipe);

    Task<IEnumerable<SearchItemByItemCategoryResult>> SearchItemByItemCategoryAsync(Guid itemCategoryId);

    Task<IEnumerable<IngredientQuantityType>> GetAllIngredientQuantityTypes();

    Task<IEnumerable<ShoppingListStore>> GetAllActiveStoresForShoppingListAsync();

    Task<ShoppingListModel> GetActiveShoppingListByStoreIdAsync(Guid storeId);

    Task<IEnumerable<ItemStore>> GetAllActiveStoresForItemAsync();

    Task<IEnumerable<StoreSearchResult>> GetActiveStoresOverviewAsync();

    Task<EditedStore> GetStoreByIdAsync(Guid storeId);

    Task<IEnumerable<RecipeTag>> GetAllRecipeTagsAsync();

    Task<RecipeTag> CreateRecipeTagAsync(string name);

    Task<IEnumerable<RecipeSearchResult>> SearchRecipesByTagsAsync(IEnumerable<Guid> tagIds);

    Task<IEnumerable<AddToShoppingListItem>> GetItemAmountsForOneServingAsync(Guid recipeId);

    Task AddItemsToShoppingListsAsync(IEnumerable<AddToShoppingListItem> items);

    Task<TemporaryShoppingListItem> AddTemporaryItemToShoppingListAsync(AddTemporaryItemToShoppingListRequest request);

    Task DeleteStoreAsync(Guid storeId);
}