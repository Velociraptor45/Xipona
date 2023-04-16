using ProjectHermes.ShoppingList.Api.Client;
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
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ItemCategories;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.Manufacturers;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IngredientQuantityType = ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States.IngredientQuantityType;
using ItemStore = ProjectHermes.ShoppingList.Frontend.Redux.Items.States.ItemStore;
using ShoppingListStore = ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States.ShoppingListStore;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;

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

    public async Task UpdateItemAsync(UpdateItemRequest request)
    {
        var contract = _converters.ToContract<EditedItem, UpdateItemContract>(request.Item);
        await _client.UpdateItemAsync(request.Item.Id, contract);
    }

    public async Task UpdateItemPriceAsync(UpdateItemPriceRequest request)
    {
        var contract = _converters.ToContract<UpdateItemPriceRequest, UpdateItemPriceContract>(request);
        await _client.UpdateItemPriceAsync(request.ItemId, contract);
    }

    public async Task UpdateItemWithTypesAsync(UpdateItemWithTypesRequest request)
    {
        var contract = _converters.ToContract<EditedItem, UpdateItemWithTypesContract>(request.Item);
        await _client.UpdateItemWithTypesAsync(request.Item.Id, contract);
    }

    public async Task ModifyItemAsync(ModifyItemRequest request)
    {
        var contract = _converters.ToContract<EditedItem, ModifyItemContract>(request.Item);
        await _client.ModifyItemAsync(request.Item.Id, contract);
    }

    public async Task ModifyItemWithTypesAsync(ModifyItemWithTypesRequest request)
    {
        var contract = _converters.ToContract<ModifyItemWithTypesRequest, ModifyItemWithTypesContract>(request);
        await _client.ModifyItemWithTypesAsync(request.Item.Id, contract);
    }

    public async Task CreateItemAsync(CreateItemRequest request)
    {
        var contract = _converters.ToContract<EditedItem, CreateItemContract>(request.Item);
        await _client.CreateItemAsync(contract);
    }

    public async Task CreateItemWithTypesAsync(CreateItemWithTypesRequest request)
    {
        var contract = _converters.ToContract<EditedItem, CreateItemWithTypesContract>(request.Item);
        await _client.CreateItemWithTypesAsync(contract);
    }

    public async Task DeleteItemAsync(DeleteItemRequest request)
    {
        await _client.DeleteItemAsync(request.ItemId);
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

    public async Task<IEnumerable<ItemSearchResult>> SearchItemsAsync(string searchInput)
    {
        var result = await _client.SearchItemsAsync(searchInput);

        return result is null ?
            Enumerable.Empty<ItemSearchResult>() :
            result.Select(_converters.ToDomain<SearchItemResultContract, ItemSearchResult>);
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

    public async Task CreateTemporaryItem(CreateTemporaryItemRequest request)
    {
        var contract = _converters.ToContract<CreateTemporaryItemRequest, CreateTemporaryItemContract>(request);
        await _client.CreateTemporaryItemAsync(contract);
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
        var results = await _client.SearchRecipesByTagsAsync(tagIds);
        return results is null
            ? Enumerable.Empty<RecipeSearchResult>()
            : _converters.ToDomain<RecipeSearchResultContract, RecipeSearchResult>(results);
    }
}