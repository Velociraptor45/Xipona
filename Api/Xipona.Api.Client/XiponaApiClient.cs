using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.GetItemTypePrices;
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
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemDiscount;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.PutItemInBasket;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemDiscount;
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
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AddItemToShoppingListContract = ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemToShoppingList.AddItemToShoppingListContract;
using ItemContract = ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get.ItemContract;

namespace ProjectHermes.Xipona.Api.Client
{
    /// <inheritdoc cref="IShoppingListApiClient"/>
    public class ShoppingListApiClient : IShoppingListApiClient
    {
        private readonly IShoppingListApiClient _apiClient;

        /// <summary>
        /// Sets up the API REST client and takes care of correct serialization.
        /// </summary>
        /// <param name="httpClient"></param>
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

        /// <inheritdoc/>
        public async Task<bool> IsAlive()
        {
            return await _apiClient.IsAlive();
        }

        #region ShoppingList

        /// <inheritdoc/>
        public async Task<ShoppingListContract> GetActiveShoppingListByStoreIdAsync(Guid storeId,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetActiveShoppingListByStoreIdAsync(storeId, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task RemoveItemFromShoppingListAsync(Guid id,
            RemoveItemFromShoppingListContract contract, CancellationToken cancellationToken = default)
        {
            await _apiClient.RemoveItemFromShoppingListAsync(id, contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TemporaryShoppingListItemContract> AddTemporaryItemToShoppingListAsync(Guid id,
            AddTemporaryItemToShoppingListContract contract, CancellationToken cancellationToken = default)
        {
            return await _apiClient.AddTemporaryItemToShoppingListAsync(id, contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddItemToShoppingListAsync(Guid id, AddItemToShoppingListContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.AddItemToShoppingListAsync(id, contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddItemWithTypeToShoppingListAsync(Guid id, Guid itemId, Guid itemTypeId,
            AddItemWithTypeToShoppingListContract contract, CancellationToken cancellationToken = default)
        {
            await _apiClient.AddItemWithTypeToShoppingListAsync(id, itemId, itemTypeId, contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddItemsToShoppingListsAsync(
            AddItemsToShoppingListsContract contract, CancellationToken cancellationToken = default)
        {
            await _apiClient.AddItemsToShoppingListsAsync(contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task PutItemInBasketAsync(Guid id, PutItemInBasketContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.PutItemInBasketAsync(id, contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task RemoveItemFromBasketAsync(Guid id, RemoveItemFromBasketContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.RemoveItemFromBasketAsync(id, contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task ChangeItemQuantityOnShoppingListAsync(Guid id,
            ChangeItemQuantityOnShoppingListContract contract, CancellationToken cancellationToken = default)
        {
            await _apiClient.ChangeItemQuantityOnShoppingListAsync(id, contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task FinishListAsync(Guid id, DateTimeOffset? finishedAt,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.FinishListAsync(id, finishedAt, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddItemDiscountAsync(Guid id, AddItemDiscountContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.AddItemDiscountAsync(id, contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task RemoveItemDiscountAsync(Guid id, RemoveItemDiscountContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.RemoveItemDiscountAsync(id, contract, cancellationToken);
        }

        #endregion ShoppingList

        #region Item

        /// <inheritdoc/>
        public async Task<ItemTypePricesContract> GetItemTypePricesAsync(Guid itemId, Guid storeId,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetItemTypePricesAsync(itemId, storeId, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task CreateItemAsync(CreateItemContract contract, CancellationToken cancellationToken = default)
        {
            await _apiClient.CreateItemAsync(contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task CreateItemWithTypesAsync(CreateItemWithTypesContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.CreateItemWithTypesAsync(contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task ModifyItemAsync(Guid id, ModifyItemContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.ModifyItemAsync(id, contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task ModifyItemWithTypesAsync(Guid id, ModifyItemWithTypesContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.ModifyItemWithTypesAsync(id, contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task UpdateItemAsync(Guid id, UpdateItemContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.UpdateItemAsync(id, contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task UpdateItemPriceAsync(Guid id, UpdateItemPriceContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.UpdateItemPriceAsync(id, contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task UpdateItemWithTypesAsync(Guid id, UpdateItemWithTypesContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.UpdateItemWithTypesAsync(id, contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task DeleteItemAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _apiClient.DeleteItemAsync(id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SearchItemForShoppingListResultContract>> SearchItemsForShoppingListAsync(
            Guid storeId, string searchInput, CancellationToken cancellationToken = default)
        {
            return await _apiClient.SearchItemsForShoppingListAsync(storeId, searchInput, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SearchItemByItemCategoryResultContract>> SearchItemsByItemCategoryAsync(
            Guid itemCategoryId, CancellationToken cancellationToken = default)
        {
            return await _apiClient.SearchItemsByItemCategoryAsync(itemCategoryId, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<int> GetTotalSearchResultCountAsync(string searchInput,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetTotalSearchResultCountAsync(searchInput, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SearchItemResultContract>> SearchItemsAsync(string searchInput, int page = 1,
            int pageSize = 20, CancellationToken cancellationToken = default)
        {
            return await _apiClient.SearchItemsAsync(searchInput, page, pageSize, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SearchItemResultContract>> SearchItemsByFilterAsync(IEnumerable<Guid> storeIds,
            IEnumerable<Guid> itemCategoryIds, IEnumerable<Guid> manufacturerIds,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.SearchItemsByFilterAsync(storeIds, itemCategoryIds, manufacturerIds, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ItemContract> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetAsync(id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task MakeTemporaryItemPermanentAsync(Guid id, MakeTemporaryItemPermanentContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.MakeTemporaryItemPermanentAsync(id, contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<QuantityTypeContract>> GetAllQuantityTypesAsync(
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetAllQuantityTypesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<QuantityTypeInPacketContract>> GetAllQuantityTypesInPacketAsync(
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetAllQuantityTypesInPacketAsync(cancellationToken);
        }

        #endregion Item

        #region Store

        /// <inheritdoc/>
        public async Task<StoreContract> GetStoreByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetStoreByIdAsync(id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<StoreForShoppingContract>> GetActiveStoresForShoppingAsync(
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetActiveStoresForShoppingAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<StoreForItemContract>> GetActiveStoresForItemAsync(
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetActiveStoresForItemAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<StoreSearchResultContract>> GetActiveStoresOverviewAsync(
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetActiveStoresOverviewAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<StoreContract> CreateStoreAsync(CreateStoreContract createStoreContract,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.CreateStoreAsync(createStoreContract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task ModifyStoreAsync(ModifyStoreContract modifyStoreContract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.ModifyStoreAsync(modifyStoreContract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task DeleteStoreAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _apiClient.DeleteStoreAsync(id, cancellationToken);
        }

        #endregion Store

        #region Manufacturer

        /// <inheritdoc/>
        public async Task<ManufacturerContract> GetManufacturerByIdAsync(Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetManufacturerByIdAsync(id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ManufacturerSearchResultContract>> GetManufacturerSearchResultsAsync(
            string searchInput, bool includeDeleted, CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetManufacturerSearchResultsAsync(searchInput, includeDeleted, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ManufacturerContract>> GetAllActiveManufacturersAsync(
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetAllActiveManufacturersAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ManufacturerContract> CreateManufacturerAsync(string name,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.CreateManufacturerAsync(name, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task DeleteManufacturerAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _apiClient.DeleteManufacturerAsync(id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task ModifyManufacturerAsync(ModifyManufacturerContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.ModifyManufacturerAsync(contract, cancellationToken);
        }

        #endregion Manufacturer

        #region ItemCategory

        /// <inheritdoc/>
        public async Task<ItemCategoryContract> GetItemCategoryByIdAsync(Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetItemCategoryByIdAsync(id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ItemCategorySearchResultContract>> SearchItemCategoriesByNameAsync(
            string searchInput, bool includeDeleted, CancellationToken cancellationToken = default)
        {
            return await _apiClient.SearchItemCategoriesByNameAsync(searchInput, includeDeleted, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ItemCategoryContract>> GetAllActiveItemCategoriesAsync(
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetAllActiveItemCategoriesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ItemCategoryContract> CreateItemCategoryAsync(string name,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.CreateItemCategoryAsync(name, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task DeleteItemCategoryAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _apiClient.DeleteItemCategoryAsync(id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task ModifyItemCategoryAsync(ModifyItemCategoryContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.ModifyItemCategoryAsync(contract, cancellationToken);
        }

        #endregion ItemCategory

        #region Recipe

        /// <inheritdoc/>
        public async Task<RecipeContract> GetRecipeByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetRecipeByIdAsync(id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<RecipeSearchResultContract>> SearchRecipesByNameAsync(string searchInput,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.SearchRecipesByNameAsync(searchInput, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<RecipeSearchResultContract>> SearchRecipesByTagsAsync(
            IEnumerable<Guid> tagIds, CancellationToken cancellationToken = default)
        {
            return await _apiClient.SearchRecipesByTagsAsync(tagIds, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IngredientQuantityTypeContract>> GetAllIngredientQuantityTypes(
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetAllIngredientQuantityTypes(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ItemAmountsForOneServingContract> GetItemAmountsForOneServingAsync(Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetItemAmountsForOneServingAsync(id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<RecipeContract> CreateRecipeAsync(CreateRecipeContract contract,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.CreateRecipeAsync(contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task ModifyRecipeAsync(Guid id, ModifyRecipeContract contract,
            CancellationToken cancellationToken = default)
        {
            await _apiClient.ModifyRecipeAsync(id, contract, cancellationToken);
        }

        #endregion Recipe

        #region RecipeTag

        /// <inheritdoc/>
        public async Task<IEnumerable<RecipeTagContract>> GetAllRecipeTagsAsync(
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.GetAllRecipeTagsAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<RecipeTagContract> CreateRecipeTagAsync(CreateRecipeTagContract contract,
            CancellationToken cancellationToken = default)
        {
            return await _apiClient.CreateRecipeTagAsync(contract, cancellationToken);
        }

        #endregion RecipeTag
    }
}