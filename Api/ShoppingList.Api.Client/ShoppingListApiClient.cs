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
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.ShoppingList.Api.Contracts.RecipeTags.Commands;
using ProjectHermes.ShoppingList.Api.Contracts.RecipeTags.Queries.GetAll;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
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
using System.Net.Http;
using System.Threading;
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
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateFormatHandling = DateFormatHandling.IsoDateFormat
                }
            }.For<IShoppingListApiClient>();
        }

        public async Task<bool> IsAlive()
        {
            return await _apiClient.IsAlive();
        }

        #region ShoppingListController

        public async Task<ShoppingListContract> GetActiveShoppingListByStoreIdAsync(Guid storeId,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetActiveShoppingListByStoreIdAsync(storeId, cancellationToken);
        }

        public async Task RemoveItemFromShoppingListAsync(Guid id,
            RemoveItemFromShoppingListContract contract, CancellationToken cancellationToken = default)
        {
            await _apiClient.RemoveItemFromShoppingListAsync(id, contract, cancellationToken);
        }

        public async Task AddItemToShoppingListAsync(Guid id, AddItemToShoppingListContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.AddItemToShoppingListAsync(id, contract, cancellationToken);
        }

        public async Task AddItemWithTypeToShoppingListAsync(Guid id, Guid itemId, Guid itemTypeId,
            AddItemWithTypeToShoppingListContract contract, CancellationToken cancellationToken = default)
        {
            await _apiClient.AddItemWithTypeToShoppingListAsync(id, itemId, itemTypeId, contract, cancellationToken);
        }

        public async Task PutItemInBasketAsync(Guid id, PutItemInBasketContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.PutItemInBasketAsync(id, contract, cancellationToken);
        }

        public async Task RemoveItemFromBasketAsync(Guid id, RemoveItemFromBasketContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.RemoveItemFromBasketAsync(id, contract, cancellationToken);
        }

        public async Task ChangeItemQuantityOnShoppingListAsync(Guid id,
            ChangeItemQuantityOnShoppingListContract contract, CancellationToken cancellationToken = default)
        {
            await _apiClient.ChangeItemQuantityOnShoppingListAsync(id, contract, cancellationToken);
        }

        public async Task FinishListAsync(Guid id, DateTimeOffset? finishedAt,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.FinishListAsync(id, finishedAt, cancellationToken);
        }

        #endregion ShoppingListController

        #region ItemController

        public async Task CreateItemAsync(CreateItemContract contract, CancellationToken cancellationToken = default)
        {
            await _apiClient.CreateItemAsync(contract, cancellationToken);
        }

        public async Task CreateItemWithTypesAsync(CreateItemWithTypesContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.CreateItemWithTypesAsync(contract, cancellationToken);
        }

        public async Task ModifyItemAsync(Guid id, ModifyItemContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.ModifyItemAsync(id, contract, cancellationToken);
        }

        public async Task ModifyItemWithTypesAsync(Guid id, ModifyItemWithTypesContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.ModifyItemWithTypesAsync(id, contract, cancellationToken);
        }

        public async Task UpdateItemAsync(Guid id, UpdateItemContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.UpdateItemAsync(id, contract, cancellationToken);
        }

        public async Task UpdateItemPriceAsync(Guid id, UpdateItemPriceContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.UpdateItemPriceAsync(id, contract, cancellationToken);
        }

        public async Task UpdateItemWithTypesAsync(Guid id, UpdateItemWithTypesContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.UpdateItemWithTypesAsync(id, contract, cancellationToken);
        }

        public async Task DeleteItemAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _apiClient.DeleteItemAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<SearchItemForShoppingListResultContract>> SearchItemsForShoppingListAsync(
            Guid storeId, string searchInput, CancellationToken cancellationToken = default)
        {
            return await _apiClient.SearchItemsForShoppingListAsync(storeId, searchInput, cancellationToken);
        }

        public async Task<IEnumerable<SearchItemByItemCategoryResultContract>> SearchItemsByItemCategoryAsync(
            Guid itemCategoryId, CancellationToken cancellationToken = default)
        {
            return await _apiClient.SearchItemsByItemCategoryAsync(itemCategoryId, cancellationToken);
        }

        public async Task<IEnumerable<SearchItemResultContract>> SearchItemsAsync(string searchInput,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.SearchItemsAsync(searchInput, cancellationToken);
        }

        public async Task<IEnumerable<SearchItemResultContract>> SearchItemsByFilterAsync(IEnumerable<Guid> storeIds,
            IEnumerable<Guid> itemCategoryIds, IEnumerable<Guid> manufacturerIds,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.SearchItemsByFilterAsync(storeIds, itemCategoryIds, manufacturerIds, cancellationToken);
        }

        public async Task<ItemContract> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetAsync(id, cancellationToken);
        }

        public async Task CreateTemporaryItemAsync(CreateTemporaryItemContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.CreateTemporaryItemAsync(contract, cancellationToken);
        }

        public async Task MakeTemporaryItemPermanentAsync(Guid id, MakeTemporaryItemPermanentContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.MakeTemporaryItemPermanentAsync(id, contract, cancellationToken);
        }

        public async Task<IEnumerable<QuantityTypeContract>> GetAllQuantityTypesAsync(
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetAllQuantityTypesAsync(cancellationToken);
        }

        public async Task<IEnumerable<QuantityTypeInPacketContract>> GetAllQuantityTypesInPacketAsync(
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetAllQuantityTypesInPacketAsync(cancellationToken);
        }

        #endregion ItemController

        #region StoreController

        public async Task<StoreContract> GetStoreByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetStoreByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<StoreForShoppingContract>> GetActiveStoresForShoppingAsync(
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetActiveStoresForShoppingAsync(cancellationToken);
        }

        public async Task<IEnumerable<StoreForItemContract>> GetActiveStoresForItemAsync(
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetActiveStoresForItemAsync(cancellationToken);
        }

        public async Task<IEnumerable<StoreSearchResultContract>> GetActiveStoresOverviewAsync(
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetActiveStoresOverviewAsync(cancellationToken);
        }

        public async Task<StoreContract> CreateStoreAsync(CreateStoreContract createStoreContract,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.CreateStoreAsync(createStoreContract, cancellationToken);
        }

        public async Task ModifyStoreAsync(ModifyStoreContract modifyStoreContract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.ModifyStoreAsync(modifyStoreContract, cancellationToken);
        }

        #endregion StoreController

        #region ManufacturerController

        public async Task<ManufacturerContract> GetManufacturerByIdAsync(Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetManufacturerByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<ManufacturerSearchResultContract>> GetManufacturerSearchResultsAsync(
            string searchInput, bool includeDeleted, CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetManufacturerSearchResultsAsync(searchInput, includeDeleted, cancellationToken);
        }

        public async Task<IEnumerable<ManufacturerContract>> GetAllActiveManufacturersAsync(
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetAllActiveManufacturersAsync(cancellationToken);
        }

        public async Task<ManufacturerContract> CreateManufacturerAsync(string name,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.CreateManufacturerAsync(name, cancellationToken);
        }

        public async Task DeleteManufacturerAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _apiClient.DeleteManufacturerAsync(id, cancellationToken);
        }

        public async Task ModifyManufacturerAsync(ModifyManufacturerContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.ModifyManufacturerAsync(contract, cancellationToken);
        }

        #endregion ManufacturerController

        #region ItemCategoryController

        public async Task<ItemCategoryContract> GetItemCategoryByIdAsync(Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetItemCategoryByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<ItemCategorySearchResultContract>> SearchItemCategoriesByNameAsync(
            string searchInput, bool includeDeleted, CancellationToken cancellationToken = default)
        {
            return await _apiClient.SearchItemCategoriesByNameAsync(searchInput, includeDeleted, cancellationToken);
        }

        public async Task<IEnumerable<ItemCategoryContract>> GetAllActiveItemCategoriesAsync(
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetAllActiveItemCategoriesAsync(cancellationToken);
        }

        public async Task<ItemCategoryContract> CreateItemCategoryAsync(string name,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.CreateItemCategoryAsync(name, cancellationToken);
        }

        public async Task DeleteItemCategoryAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _apiClient.DeleteItemCategoryAsync(id, cancellationToken);
        }

        public async Task ModifyItemCategoryAsync(ModifyItemCategoryContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.ModifyItemCategoryAsync(contract, cancellationToken);
        }

        #endregion ItemCategoryController

        #region RecipeController

        public async Task<RecipeContract> GetRecipeByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetRecipeByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<RecipeSearchResultContract>> SearchRecipesByNameAsync(string searchInput,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.SearchRecipesByNameAsync(searchInput, cancellationToken);
        }

        public async Task<IEnumerable<IngredientQuantityTypeContract>> GetAllIngredientQuantityTypes(
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetAllIngredientQuantityTypes(cancellationToken);
        }

        public async Task<RecipeContract> CreateRecipeAsync(CreateRecipeContract contract,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.CreateRecipeAsync(contract, cancellationToken);
        }

        public async Task ModifyRecipeAsync(Guid id, ModifyRecipeContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.ModifyRecipeAsync(id, contract, cancellationToken);
        }

        #endregion RecipeController

        #region RecipeTagController

        public async Task<IEnumerable<RecipeTagContract>> GetAllRecipeTagsAsync(
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetAllRecipeTagsAsync(cancellationToken);
        }

        public async Task<RecipeTagContract> CreateRecipeTagAsync(CreateRecipeTagContract contract,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.CreateRecipeTagAsync(contract, cancellationToken);
        }

        #endregion RecipeTagController
    }
}