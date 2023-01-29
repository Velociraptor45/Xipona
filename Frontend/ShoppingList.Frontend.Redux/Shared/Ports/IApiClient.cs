using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ShoppingList.Frontend.Redux.ItemCategories.States;
using ShoppingList.Frontend.Redux.Items.States;
using ShoppingList.Frontend.Redux.Manufacturers.States;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.ItemCategories;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.Manufacturers;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.Stores;
using ShoppingList.Frontend.Redux.ShoppingList.States;
using IngredientQuantityType = global::ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States.IngredientQuantityType;
using ItemStore = ShoppingList.Frontend.Redux.Items.States.ItemStore;
using SharedStates = ShoppingList.Frontend.Redux.Shared.States;
using ShoppingListStore = ShoppingList.Frontend.Redux.ShoppingList.States.ShoppingListStore;

namespace ShoppingList.Frontend.Redux.Shared.Ports
{
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

        Task<IEnumerable<Store>> GetAllActiveStoresAsync();

        Task<IEnumerable<SharedStates.QuantityTypeInPacket>> GetAllQuantityTypesInPacketAsync();

        Task<IEnumerable<SharedStates.QuantityType>> GetAllQuantityTypesAsync();

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

        Task CreateStoreAsync(CreateStoreRequest request);

        Task ModifyStoreAsync(ModifyStoreRequest request);

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

        Task<ShoppingListModel> GetActiveShoppingListByStoreIdAsync(Guid storeId); // todo name #298

        Task<IEnumerable<ItemStore>> GetAllActiveStoresForItemAsync();
    }
}