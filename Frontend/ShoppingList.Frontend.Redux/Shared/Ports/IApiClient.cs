using ProjectHermes.ShoppingList.Frontend.Models.ItemCategories.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Manufacturers.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Recipes.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using ShoppingList.Frontend.Redux.Manufacturers.States;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.ItemCategories;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.Manufacturers;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.Stores;
using ShoppingList.Frontend.Redux.ShoppingList.States;
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

        Task<ItemCategory> CreateItemCategoryAsync(string name);

        Task<Manufacturer> CreateManufacturerAsync(string name);

        Task CreateTemporaryItem(CreateTemporaryItemRequest request);

        Task DeleteItemAsync(DeleteItemRequest request);

        Task FinishListAsync(FinishListRequest request);

        Task<IEnumerable<ItemCategory>> GetAllActiveItemCategoriesAsync();

        Task<IEnumerable<Manufacturer>> GetAllActiveManufacturersAsync();

        Task<IEnumerable<Store>> GetAllActiveStoresAsync();

        Task<IEnumerable<SharedStates.QuantityTypeInPacket>> GetAllQuantityTypesInPacketAsync();

        Task<IEnumerable<SharedStates.QuantityType>> GetAllQuantityTypesAsync();

        Task<Item> GetItemByIdAsync(Guid itemId);

        Task<IEnumerable<SearchItemResult>> SearchItemsAsync(string searchInput);

        Task<IEnumerable<SearchItemResult>> SearchItemsByFilterAsync(IEnumerable<Guid> storeIds,
            IEnumerable<Guid> itemCategoryIds, IEnumerable<Guid> manufacturerIds);

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

        Task<Manufacturer> GetManufacturerByIdAsync(Guid id);

        Task DeleteManufacturerAsync(Guid id);

        Task ModifyManufacturerAsync(ModifyManufacturerRequest request);

        Task<ItemCategory> GetItemCategoryByIdAsync(Guid id);

        Task<IEnumerable<ItemCategorySearchResult>> GetItemCategoriesSearchResultsAsync(string searchInput);

        Task DeleteItemCategoryAsync(Guid id);

        Task ModifyItemCategoryAsync(ModifyItemCategoryRequest request);

        Task UpdateItemPriceAsync(UpdateItemPriceRequest request);

        Task<Recipe> GetRecipeByIdAsync(Guid recipeId);

        Task<IEnumerable<RecipeSearchResult>> SearchRecipesByNameAsync(string searchInput);

        Task<Recipe> CreateRecipeAsync(Recipe recipe);

        Task ModifyRecipeAsync(Recipe recipe);

        Task<IEnumerable<SearchItemByItemCategoryResult>> SearchItemByItemCategoryAsync(Guid itemCategoryId);

        Task<IEnumerable<IngredientQuantityType>> GetAllIngredientQuantityTypes();

        Task<IEnumerable<ShoppingListStore>> GetAllActiveStoresForShoppingListAsync();

        Task<ShoppingListModel> GetActiveShoppingListByStoreIdAsync(Guid storeId); // todo name
    }
}