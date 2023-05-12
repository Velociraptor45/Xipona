using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ItemCategories;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.Manufacturers;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;
using IngredientQuantityType = ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States.IngredientQuantityType;
using ItemStore = ProjectHermes.ShoppingList.Frontend.Redux.Items.States.ItemStore;
using ShoppingListStore = ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States.ShoppingListStore;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;

public interface IApiClient
{
    Task AddItemToShoppingListAsync(AddItemToShoppingListRequest request);

    Task ModifyItemAsync(ModifyItemRequest request);

    Task ChangeItemQuantityOnShoppingListAsync(ChangeItemQuantityOnShoppingListRequest request);

    Task CreateItemAsync(CreateItemRequest request);

    Task<EditedItemCategory> CreateItemCategoryAsync(string name);

    Task<EditedManufacturer> CreateManufacturerAsync(string name);

    Task CreateTemporaryItem(CreateTemporaryItemRequest request);

    Task DeleteItemAsync(DeleteItemRequest request);

    Task FinishListAsync(FinishListRequest request);

    Task<IEnumerable<QuantityTypeInPacket>> GetAllQuantityTypesInPacketAsync();

    Task<IEnumerable<QuantityType>> GetAllQuantityTypesAsync();

    Task<EditedItem> GetItemByIdAsync(Guid itemId);

    Task<IEnumerable<ItemSearchResult>> SearchItemsAsync(string searchInput);

    Task<IEnumerable<SearchItemForShoppingListResult>> SearchItemsForShoppingListAsync(string searchInput,
        Guid storeId, CancellationToken cancellationToken);

    Task IsAliveAsync();

    Task MakeTemporaryItemPermanent(MakeTemporaryItemPermanentRequest request);

    Task PutItemInBasketAsync(PutItemInBasketRequest request);

    Task RemoveItemFromBasketAsync(RemoveItemFromBasketRequest request);

    Task RemoveItemFromShoppingListAsync(RemoveItemFromShoppingListRequest request);

    Task UpdateItemAsync(UpdateItemRequest request);

    Task CreateStoreAsync(EditedStore store);

    Task ModifyStoreAsync(EditedStore store);

    Task ModifyItemWithTypesAsync(ModifyItemWithTypesRequest request);

    Task AddItemWithTypeToShoppingListAsync(AddItemWithTypeToShoppingListRequest request);

    Task UpdateItemWithTypesAsync(UpdateItemWithTypesRequest request);

    Task CreateItemWithTypesAsync(CreateItemWithTypesRequest request);

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
}