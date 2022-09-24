using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.Shared;
using RestEase;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Client
{
    public class ShoppingListApiClient : IShoppingListApiClient
    {
        private readonly IShoppingListApiClient _apiClient;

        public ShoppingListApiClient(HttpClient httpClient)
        {
            _apiClient = new RestClient(httpClient)
            {
                JsonSerializerSettings =
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            }.For<IShoppingListApiClient>();
        }

        public async Task<bool> IsAlive()
        {
            return await _apiClient.IsAlive();
        }

        #region ShoppingListController

        public async Task<ShoppingListContract> GetActiveShoppingListByStoreIdAsync(Guid storeId)
        {
            return await _apiClient.GetActiveShoppingListByStoreIdAsync(storeId);
        }

        public async Task RemoveItemFromShoppingListAsync(Guid id,
            RemoveItemFromShoppingListContract contract)
        {
            await _apiClient.RemoveItemFromShoppingListAsync(id, contract);
        }

        public async Task AddItemToShoppingListAsync(Guid id, AddItemToShoppingListContract contract)
        {
            await _apiClient.AddItemToShoppingListAsync(id, contract);
        }

        public async Task AddItemWithTypeToShoppingListAsync(Guid id, Guid itemId, Guid itemTypeId,
            AddItemWithTypeToShoppingListContract contract)
        {
            await _apiClient.AddItemWithTypeToShoppingListAsync(id, itemId, itemTypeId, contract);
        }

        public async Task PutItemInBasketAsync(Guid id, PutItemInBasketContract contract)
        {
            await _apiClient.PutItemInBasketAsync(id, contract);
        }

        public async Task RemoveItemFromBasketAsync(Guid id, RemoveItemFromBasketContract contract)
        {
            await _apiClient.RemoveItemFromBasketAsync(id, contract);
        }

        public async Task ChangeItemQuantityOnShoppingListAsync(Guid id, ChangeItemQuantityOnShoppingListContract contract)
        {
            await _apiClient.ChangeItemQuantityOnShoppingListAsync(id, contract);
        }

        public async Task FinishListAsync(Guid id)
        {
            await _apiClient.FinishListAsync(id);
        }

        #endregion ShoppingListController

        #region ItemController

        public async Task CreateItemAsync(CreateItemContract contract)
        {
            await _apiClient.CreateItemAsync(contract);
        }

        public async Task CreateItemWithTypesAsync(CreateItemWithTypesContract contract)
        {
            await _apiClient.CreateItemWithTypesAsync(contract);
        }

        public async Task ModifyItemAsync(Guid id, ModifyItemContract contract)
        {
            await _apiClient.ModifyItemAsync(id, contract);
        }

        public async Task ModifyItemWithTypesAsync(Guid id, ModifyItemWithTypesContract contract)
        {
            await _apiClient.ModifyItemWithTypesAsync(id, contract);
        }

        public async Task UpdateItemAsync(Guid id, UpdateItemContract contract)
        {
            await _apiClient.UpdateItemAsync(id, contract);
        }

        public async Task UpdateItemPriceAsync(Guid id, UpdateItemPriceContract contract)
        {
            await _apiClient.UpdateItemPriceAsync(id, contract);
        }

        public async Task UpdateItemWithTypesAsync(Guid id, UpdateItemWithTypesContract contract)
        {
            await _apiClient.UpdateItemWithTypesAsync(id, contract);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            await _apiClient.DeleteItemAsync(id);
        }

        public async Task<IEnumerable<SearchItemForShoppingListResultContract>> SearchItemsForShoppingListAsync(
            Guid storeId, string searchInput)
        {
            return await _apiClient.SearchItemsForShoppingListAsync(storeId, searchInput);
        }

        public async Task<IEnumerable<SearchItemByItemCategoryResultContract>> SearchItemsByItemCategoryAsync(
            Guid itemCategoryId)
        {
            return await _apiClient.SearchItemsByItemCategoryAsync(itemCategoryId);
        }

        public async Task<IEnumerable<SearchItemResultContract>> SearchItemsAsync(string searchInput)
        {
            return await _apiClient.SearchItemsAsync(searchInput);
        }

        public async Task<IEnumerable<SearchItemResultContract>> SearchItemsByFilterAsync(IEnumerable<Guid> storeIds,
            IEnumerable<Guid> itemCategoryIds, IEnumerable<Guid> manufacturerIds)
        {
            return await _apiClient.SearchItemsByFilterAsync(storeIds, itemCategoryIds, manufacturerIds);
        }

        public async Task<ItemContract> GetAsync(Guid id)
        {
            return await _apiClient.GetAsync(id);
        }

        public async Task CreateTemporaryItemAsync(CreateTemporaryItemContract contract)
        {
            await _apiClient.CreateTemporaryItemAsync(contract);
        }

        public async Task MakeTemporaryItemPermanentAsync(Guid id, MakeTemporaryItemPermanentContract contract)
        {
            await _apiClient.MakeTemporaryItemPermanentAsync(id, contract);
        }

        public async Task<IEnumerable<QuantityTypeContract>> GetAllQuantityTypesAsync()
        {
            return await _apiClient.GetAllQuantityTypesAsync();
        }

        public async Task<IEnumerable<QuantityTypeInPacketContract>> GetAllQuantityTypesInPacketAsync()
        {
            return await _apiClient.GetAllQuantityTypesInPacketAsync();
        }

        #endregion ItemController

        #region StoreController

        public async Task<IEnumerable<ActiveStoreContract>> GetAllActiveStoresAsync()
        {
            return await _apiClient.GetAllActiveStoresAsync();
        }

        public async Task<StoreContract> CreateStoreAsync(CreateStoreContract createStoreContract)
        {
            return await _apiClient.CreateStoreAsync(createStoreContract);
        }

        public async Task UpdateStoreAsync(UpdateStoreContract updateStoreContract)
        {
            await _apiClient.UpdateStoreAsync(updateStoreContract);
        }

        #endregion StoreController

        #region ManufacturerController

        public async Task<ManufacturerContract> GetManufacturerByIdAsync(Guid id)
        {
            return await _apiClient.GetManufacturerByIdAsync(id);
        }

        public async Task<IEnumerable<ManufacturerSearchResultContract>> GetManufacturerSearchResultsAsync(
            string searchInput, bool includeDeleted)
        {
            return await _apiClient.GetManufacturerSearchResultsAsync(searchInput, includeDeleted);
        }

        public async Task<IEnumerable<ManufacturerContract>> GetAllActiveManufacturersAsync()
        {
            return await _apiClient.GetAllActiveManufacturersAsync();
        }

        public async Task<ManufacturerContract> CreateManufacturerAsync(string name)
        {
            return await _apiClient.CreateManufacturerAsync(name);
        }

        public async Task DeleteManufacturerAsync(Guid id)
        {
            await _apiClient.DeleteManufacturerAsync(id);
        }

        public async Task ModifyManufacturerAsync(ModifyManufacturerContract contract)
        {
            await _apiClient.ModifyManufacturerAsync(contract);
        }

        #endregion ManufacturerController

        #region ItemCategoryController

        public async Task<ItemCategoryContract> GetItemCategoryByIdAsync(Guid id)
        {
            return await _apiClient.GetItemCategoryByIdAsync(id);
        }

        public async Task<IEnumerable<ItemCategorySearchResultContract>> SearchItemCategoriesByNameAsync(string searchInput,
            bool includeDeleted)
        {
            return await _apiClient.SearchItemCategoriesByNameAsync(searchInput, includeDeleted);
        }

        public async Task<IEnumerable<ItemCategoryContract>> GetAllActiveItemCategoriesAsync()
        {
            return await _apiClient.GetAllActiveItemCategoriesAsync();
        }

        public async Task<ItemCategoryContract> CreateItemCategoryAsync(string name)
        {
            return await _apiClient.CreateItemCategoryAsync(name);
        }

        public async Task DeleteItemCategoryAsync(Guid id)
        {
            await _apiClient.DeleteItemCategoryAsync(id);
        }

        public async Task ModifyItemCategoryAsync(ModifyItemCategoryContract contract)
        {
            await _apiClient.ModifyItemCategoryAsync(contract);
        }

        #endregion ItemCategoryController

        #region RecipeController

        public async Task<RecipeContract> GetRecipeByIdAsync(Guid recipeId)
        {
            return await _apiClient.GetRecipeByIdAsync(recipeId);
        }

        public async Task<IEnumerable<RecipeSearchResultContract>> SearchRecipesByNameAsync(string searchInput)
        {
            return await _apiClient.SearchRecipesByNameAsync(searchInput);
        }

        public async Task<RecipeContract> CreateRecipeAsync(CreateRecipeContract contract)
        {
            return await _apiClient.CreateRecipeAsync(contract);
        }

        public async Task ModifyRecipeAsync(Guid id, ModifyRecipeContract contract)
        {
            await _apiClient.ModifyRecipeAsync(id, contract);
        }

        #endregion RecipeController
    }
}