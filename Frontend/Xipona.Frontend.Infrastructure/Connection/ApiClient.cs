using ProjectHermes.Xipona.Api.Client;
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
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using ProjectHermes.Xipona.Frontend.Redux.Manufacturers.States;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ItemCategories;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.Items;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.Manufacturers;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.Stores.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AddItemToShoppingListContract = ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemToShoppingList.AddItemToShoppingListContract;
using IngredientQuantityType = ProjectHermes.Xipona.Frontend.Redux.Recipes.States.IngredientQuantityType;
using ItemStore = ProjectHermes.Xipona.Frontend.Redux.Items.States.ItemStore;
using ShoppingListStore = ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.ShoppingListStore;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Connection;

public class ApiClient : IApiClient
{
    private readonly IShoppingListApiClient _client;
    private readonly IApiConverters _converters;

    public ApiClient(IShoppingListApiClient client, IApiConverters converters)
    {
        _client = client;
        _converters = converters;
    }

    public async Task IsAliveAsync()
    {
        _ = await _client.IsAlive();
    }

    public async Task PutItemInBasketAsync(PutItemInBasketRequest request)
    {
        var contract = _converters.ToContract<PutItemInBasketRequest, PutItemInBasketContract>(request);
        await _client.PutItemInBasketAsync(request.ShoppingListId, contract);
    }

    public async Task RemoveItemFromBasketAsync(RemoveItemFromBasketRequest request)
    {
        var contract = _converters.ToContract<RemoveItemFromBasketRequest, RemoveItemFromBasketContract>(request);
        await _client.RemoveItemFromBasketAsync(request.ShoppingListId, contract);
    }

    public async Task ChangeItemQuantityOnShoppingListAsync(ChangeItemQuantityOnShoppingListRequest request)
    {
        var contract =
            _converters
                .ToContract<ChangeItemQuantityOnShoppingListRequest, ChangeItemQuantityOnShoppingListContract>(
                    request);
        await _client.ChangeItemQuantityOnShoppingListAsync(request.ShoppingListId, contract);
    }

    public async Task FinishListAsync(FinishListRequest request)
    {
        await _client.FinishListAsync(request.ShoppingListId, request.FinishedAt);
    }

    public async Task RemoveItemFromShoppingListAsync(RemoveItemFromShoppingListRequest request)
    {
        var contract =
            _converters.ToContract<RemoveItemFromShoppingListRequest, RemoveItemFromShoppingListContract>(request);
        await _client.RemoveItemFromShoppingListAsync(request.ShoppingListId, contract);
    }

    public async Task<TemporaryShoppingListItem> AddTemporaryItemToShoppingListAsync(AddTemporaryItemToShoppingListRequest request)
    {
        var contract = _converters.ToContract<AddTemporaryItemToShoppingListRequest, AddTemporaryItemToShoppingListContract>(request);
        var tempItem = await _client.AddTemporaryItemToShoppingListAsync(request.ShoppingListId, contract);

        return _converters.ToDomain<TemporaryShoppingListItemContract, TemporaryShoppingListItem>(tempItem);
    }

    public async Task AddItemToShoppingListAsync(AddItemToShoppingListRequest request)
    {
        var contract = _converters.ToContract<AddItemToShoppingListRequest, AddItemToShoppingListContract>(request);
        await _client.AddItemToShoppingListAsync(request.ShoppingListId, contract);
    }

    public async Task AddItemWithTypeToShoppingListAsync(AddItemWithTypeToShoppingListRequest request)
    {
        var contract =
            _converters.ToContract<AddItemWithTypeToShoppingListRequest, AddItemWithTypeToShoppingListContract>(
                request);
        await _client.AddItemWithTypeToShoppingListAsync(request.ShoppingListId, request.ItemId, request.ItemTypeId,
            contract);
    }

    public async Task UpdateItemAsync(EditedItem item)
    {
        var contract = _converters.ToContract<EditedItem, UpdateItemContract>(item);
        await _client.UpdateItemAsync(item.Id, contract);
    }

    public async Task UpdateItemPriceAsync(UpdateItemPriceRequest request)
    {
        var contract = _converters.ToContract<UpdateItemPriceRequest, UpdateItemPriceContract>(request);
        await _client.UpdateItemPriceAsync(request.ItemId, contract);
    }

    public async Task UpdateItemWithTypesAsync(EditedItem item)
    {
        var contract = _converters.ToContract<EditedItem, UpdateItemWithTypesContract>(item);
        await _client.UpdateItemWithTypesAsync(item.Id, contract);
    }

    public async Task ModifyItemAsync(EditedItem item)
    {
        var contract = _converters.ToContract<EditedItem, ModifyItemContract>(item);
        await _client.ModifyItemAsync(item.Id, contract);
    }

    public async Task ModifyItemWithTypesAsync(EditedItem item)
    {
        var contract = _converters.ToContract<EditedItem, ModifyItemWithTypesContract>(item);
        await _client.ModifyItemWithTypesAsync(item.Id, contract);
    }

    public async Task CreateItemAsync(EditedItem item)
    {
        var contract = _converters.ToContract<EditedItem, CreateItemContract>(item);
        await _client.CreateItemAsync(contract);
    }

    public async Task CreateItemWithTypesAsync(EditedItem item)
    {
        var contract = _converters.ToContract<EditedItem, CreateItemWithTypesContract>(item);
        await _client.CreateItemWithTypesAsync(contract);
    }

    public async Task DeleteItemAsync(Guid itemId)
    {
        await _client.DeleteItemAsync(itemId);
    }

    public async Task<EditedManufacturer> CreateManufacturerAsync(string name)
    {
        var result = await _client.CreateManufacturerAsync(name);
        return _converters.ToDomain<ManufacturerContract, EditedManufacturer>(result);
    }

    public async Task<EditedItemCategory> CreateItemCategoryAsync(string name)
    {
        var result = await _client.CreateItemCategoryAsync(name);
        return _converters.ToDomain<ItemCategoryContract, EditedItemCategory>(result);
    }

    public async Task<ShoppingListModel> GetActiveShoppingListByStoreIdAsync(Guid storeId)
    {
        var list = await _client.GetActiveShoppingListByStoreIdAsync(storeId);
        return _converters.ToDomain<ShoppingListContract, ShoppingListModel>(list);
    }

    public async Task<IEnumerable<ShoppingListStore>> GetAllActiveStoresForShoppingListAsync()
    {
        var contracts = await _client.GetActiveStoresForShoppingAsync();

        return contracts is null ?
            Enumerable.Empty<ShoppingListStore>() :
            contracts.Select(_converters.ToDomain<StoreForShoppingContract, ShoppingListStore>);
    }

    public async Task<IEnumerable<ItemStore>> GetAllActiveStoresForItemAsync()
    {
        var contracts = await _client.GetActiveStoresForItemAsync();

        return contracts is null ?
            Enumerable.Empty<ItemStore>() :
            contracts.Select(_converters.ToDomain<StoreForItemContract, ItemStore>);
    }

    public async Task<IEnumerable<StoreSearchResult>> GetActiveStoresOverviewAsync()
    {
        var contracts = await _client.GetActiveStoresOverviewAsync();

        return contracts is null
            ? Enumerable.Empty<StoreSearchResult>()
            : contracts.Select(_converters.ToDomain<StoreSearchResultContract, StoreSearchResult>);
    }

    public async Task<EditedStore> GetStoreByIdAsync(Guid storeId)
    {
        var contract = await _client.GetStoreByIdAsync(storeId);

        return _converters.ToDomain<StoreContract, EditedStore>(contract);
    }

    public async Task<IEnumerable<SearchItemForShoppingListResult>> SearchItemsForShoppingListAsync(
        string searchInput, Guid storeId, CancellationToken cancellationToken)
    {
        var result = await _client.SearchItemsForShoppingListAsync(storeId, searchInput, cancellationToken);
        if (result is null)
            return Enumerable.Empty<SearchItemForShoppingListResult>();

        return result
            .Select(_converters.ToDomain<SearchItemForShoppingListResultContract, SearchItemForShoppingListResult>);
    }

    public async Task<int> GetTotalSearchResultCountAsync(string searchInput)
    {
        return await _client.GetTotalSearchResultCountAsync(searchInput);
    }

    public async Task<IEnumerable<ItemSearchResult>> SearchItemsAsync(string searchInput, int page, int pageSize)
    {
        var result = await _client.SearchItemsAsync(searchInput, page, pageSize);

        return result is null
            ? Enumerable.Empty<ItemSearchResult>()
            : result.Select(_converters.ToDomain<SearchItemResultContract, ItemSearchResult>);
    }

    public async Task<EditedItem> GetItemByIdAsync(Guid itemId)
    {
        var result = await _client.GetAsync(itemId);
        return _converters.ToDomain<ItemContract, EditedItem>(result);
    }

    public async Task<IEnumerable<QuantityType>> GetAllQuantityTypesAsync()
    {
        var result = await _client.GetAllQuantityTypesAsync();
        return result is null ?
            Enumerable.Empty<QuantityType>() :
            result.Select(_converters.ToDomain<QuantityTypeContract, QuantityType>);
    }

    public async Task<IEnumerable<QuantityTypeInPacket>> GetAllQuantityTypesInPacketAsync()
    {
        var result = await _client.GetAllQuantityTypesInPacketAsync();
        return result is null ?
            Enumerable.Empty<QuantityTypeInPacket>() :
            result.Select(_converters.ToDomain<QuantityTypeInPacketContract, QuantityTypeInPacket>);
    }

    public async Task MakeTemporaryItemPermanent(MakeTemporaryItemPermanentRequest request)
    {
        var contract =
            _converters.ToContract<MakeTemporaryItemPermanentRequest, MakeTemporaryItemPermanentContract>(request);
        await _client.MakeTemporaryItemPermanentAsync(request.ItemId, contract);
    }

    public async Task CreateStoreAsync(EditedStore store)
    {
        var contract = _converters.ToContract<EditedStore, CreateStoreContract>(store);
        await _client.CreateStoreAsync(contract);
    }

    public async Task ModifyStoreAsync(EditedStore store)
    {
        await _client.ModifyStoreAsync(_converters.ToContract<EditedStore, ModifyStoreContract>(store));
    }

    public async Task<IEnumerable<ManufacturerSearchResult>> GetManufacturerSearchResultsAsync(string searchInput)
    {
        var result = await _client.GetManufacturerSearchResultsAsync(searchInput, false);

        return result is null
            ? Enumerable.Empty<ManufacturerSearchResult>()
            : result.Select(_converters.ToDomain<ManufacturerSearchResultContract, ManufacturerSearchResult>);
    }

    public async Task<EditedManufacturer> GetManufacturerByIdAsync(Guid id)
    {
        var result = await _client.GetManufacturerByIdAsync(id);

        return _converters.ToDomain<ManufacturerContract, EditedManufacturer>(result);
    }

    public async Task DeleteManufacturerAsync(Guid id)
    {
        await _client.DeleteManufacturerAsync(id);
    }

    public async Task ModifyManufacturerAsync(ModifyManufacturerRequest request)
    {
        var contract = _converters.ToContract<ModifyManufacturerRequest, ModifyManufacturerContract>(request);
        await _client.ModifyManufacturerAsync(contract);
    }

    public async Task<EditedItemCategory> GetItemCategoryByIdAsync(Guid id)
    {
        var result = await _client.GetItemCategoryByIdAsync(id);
        return _converters.ToDomain<ItemCategoryContract, EditedItemCategory>(result);
    }

    public async Task<IEnumerable<ItemCategorySearchResult>> GetItemCategorySearchResultsAsync(string searchInput)
    {
        var results = await _client.SearchItemCategoriesByNameAsync(searchInput, false);

        return results is null
            ? Enumerable.Empty<ItemCategorySearchResult>()
            : results.Select(_converters.ToDomain<ItemCategorySearchResultContract, ItemCategorySearchResult>);
    }

    public async Task DeleteItemCategoryAsync(Guid id)
    {
        await _client.DeleteItemCategoryAsync(id);
    }

    public async Task ModifyItemCategoryAsync(ModifyItemCategoryRequest request)
    {
        var contract = _converters.ToContract<ModifyItemCategoryRequest, ModifyItemCategoryContract>(request);
        await _client.ModifyItemCategoryAsync(contract);
    }

    public async Task<IEnumerable<SearchItemByItemCategoryResult>> SearchItemByItemCategoryAsync(Guid itemCategoryId)
    {
        var results = await _client.SearchItemsByItemCategoryAsync(itemCategoryId);
        return results is null
            ? Enumerable.Empty<SearchItemByItemCategoryResult>()
            : _converters.ToDomain<SearchItemByItemCategoryResultContract, SearchItemByItemCategoryResult>(results);
    }

    public async Task<EditedRecipe> GetRecipeByIdAsync(Guid recipeId)
    {
        var result = await _client.GetRecipeByIdAsync(recipeId);
        return _converters.ToDomain<RecipeContract, EditedRecipe>(result);
    }

    public async Task<IEnumerable<RecipeSearchResult>> SearchRecipesByNameAsync(string searchInput)
    {
        var results = await _client.SearchRecipesByNameAsync(searchInput);
        return results is null
            ? Enumerable.Empty<RecipeSearchResult>()
            : _converters.ToDomain<RecipeSearchResultContract, RecipeSearchResult>(results);
    }

    public async Task<EditedRecipe> CreateRecipeAsync(EditedRecipe recipe)
    {
        var contract = _converters.ToContract<EditedRecipe, CreateRecipeContract>(recipe);
        var result = await _client.CreateRecipeAsync(contract);
        return _converters.ToDomain<RecipeContract, EditedRecipe>(result);
    }

    public async Task ModifyRecipeAsync(EditedRecipe recipe)
    {
        var contract = _converters.ToContract<EditedRecipe, ModifyRecipeContract>(recipe);
        await _client.ModifyRecipeAsync(recipe.Id, contract);
    }

    public async Task<IEnumerable<IngredientQuantityType>> GetAllIngredientQuantityTypes()
    {
        var types = await _client.GetAllIngredientQuantityTypes();
        return types is null
            ? Enumerable.Empty<IngredientQuantityType>()
            : _converters.ToDomain<IngredientQuantityTypeContract, IngredientQuantityType>(types);
    }

    public async Task<IEnumerable<RecipeTag>> GetAllRecipeTagsAsync()
    {
        var tags = await _client.GetAllRecipeTagsAsync();
        return tags is null
            ? Enumerable.Empty<RecipeTag>()
            : _converters.ToDomain<RecipeTagContract, RecipeTag>(tags);
    }

    public async Task<RecipeTag> CreateRecipeTagAsync(string name)
    {
        var contract = new CreateRecipeTagContract(name);
        var result = await _client.CreateRecipeTagAsync(contract);
        return _converters.ToDomain<RecipeTagContract, RecipeTag>(result);
    }

    public async Task<IEnumerable<RecipeSearchResult>> SearchRecipesByTagsAsync(IEnumerable<Guid> tagIds)
    {
        var results = await _client.SearchRecipesByTagsAsync(tagIds.ToArray());
        return results is null
            ? Enumerable.Empty<RecipeSearchResult>()
            : _converters.ToDomain<RecipeSearchResultContract, RecipeSearchResult>(results);
    }

    public async Task<IEnumerable<AddToShoppingListItem>> GetItemAmountsForOneServingAsync(Guid recipeId)
    {
        var result = await _client.GetItemAmountsForOneServingAsync(recipeId);
        return _converters.ToDomain<ItemAmountForOneServingContract, AddToShoppingListItem>(result.Items);
    }

    public async Task AddItemsToShoppingListsAsync(IEnumerable<AddToShoppingListItem> items)
    {
        var contract = _converters.ToContract<IEnumerable<AddToShoppingListItem>, AddItemsToShoppingListsContract>(items);
        await _client.AddItemsToShoppingListsAsync(contract);
    }

    public async Task DeleteStoreAsync(Guid storeId)
    {
        await _client.DeleteStoreAsync(storeId);
    }

    public async Task<IEnumerable<ItemTypePrice>> GetItemTypePricesAsync(Guid itemId, Guid storeId)
    {
        var result = await _client.GetItemTypePricesAsync(itemId, storeId);
        return _converters.ToDomain<ItemTypePriceContract, ItemTypePrice>(result.Prices);
    }

    public async Task AddItemDiscountAsync(Guid shoppingListId, Guid itemId, Guid? itemTypeId, decimal discount)
    {
        var contract = new AddItemDiscountContract(discount, itemId, itemTypeId);
        await _client.AddItemDiscountAsync(shoppingListId, contract);
    }

    public async Task RemoveItemDiscountAsync(Guid shoppingListId, Guid itemId, Guid? itemTypeId)
    {
        var contract = new RemoveItemDiscountContract(itemId, itemTypeId);
        await _client.RemoveItemDiscountAsync(shoppingListId, contract);
    }
}