using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xipona.Api.Client;
using Xipona.Api.Contracts.Common.Queries;
using Xipona.Api.Contracts.ItemCategories.Commands;
using Xipona.Api.Contracts.ItemCategories.Queries;
using Xipona.Api.Contracts.Items.Commands.CreateItem;
using Xipona.Api.Contracts.Items.Commands.CreateItemWithTypes;
using Xipona.Api.Contracts.Items.Commands.MakeTemporaryItemPermanent;
using Xipona.Api.Contracts.Items.Commands.MergeItems;
using Xipona.Api.Contracts.Items.Commands.ModifyItem;
using Xipona.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using Xipona.Api.Contracts.Items.Commands.UpdateItem;
using Xipona.Api.Contracts.Items.Commands.UpdateItemPrice;
using Xipona.Api.Contracts.Items.Commands.UpdateItemWithTypes;
using Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using Xipona.Api.Contracts.Items.Queries.Get;
using Xipona.Api.Contracts.Items.Queries.GetItemTypePrices;
using Xipona.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using Xipona.Api.Contracts.Items.Queries.SearchItemsForMerge;
using Xipona.Api.Contracts.Items.Queries.SearchItemsForShoppingLists;
using Xipona.Api.Contracts.Items.Queries.Shared;
using Xipona.Api.Contracts.Manufacturers.Commands;
using Xipona.Api.Contracts.Manufacturers.Queries;
using Xipona.Api.Contracts.Recipes.Commands.CreateRecipe;
using Xipona.Api.Contracts.Recipes.Commands.ModifyRecipe;
using Xipona.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes;
using Xipona.Api.Contracts.Recipes.Queries.Get;
using Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing;
using Xipona.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using Xipona.Api.Contracts.RecipeTags.Commands;
using Xipona.Api.Contracts.RecipeTags.Queries.GetAll;
using Xipona.Api.Contracts.ShoppingLists.Commands.AddItemDiscount;
using Xipona.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists;
using Xipona.Api.Contracts.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using Xipona.Api.Contracts.ShoppingLists.Commands.AddShoppingListDiscount;
using Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using Xipona.Api.Contracts.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using Xipona.Api.Contracts.ShoppingLists.Commands.PutItemInBasket;
using Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemDiscount;
using Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromBasket;
using Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromShoppingList;
using Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using Xipona.Api.Contracts.Stores.Commands.CreateStore;
using Xipona.Api.Contracts.Stores.Commands.ModifyStore;
using Xipona.Api.Contracts.Stores.Queries.Get;
using Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForItem;
using Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForShopping;
using Xipona.Api.Contracts.Stores.Queries.GetActiveStoresOverview;
using Xipona.Api.Contracts.Users.Commands.AllCurrencies;
using Xipona.Api.Contracts.Users.Commands.Login;
using Xipona.Api.Contracts.Users.Commands.UpdateGeneralSettings;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.ItemCategories.States;
using Xipona.Frontend.Redux.Items.States;
using Xipona.Frontend.Redux.Items.States.Merges;
using Xipona.Frontend.Redux.Manufacturers.States;
using Xipona.Frontend.Redux.Recipes.States;
using Xipona.Frontend.Redux.Shared.Ports;
using Xipona.Frontend.Redux.Shared.Ports.Requests.ItemCategories;
using Xipona.Frontend.Redux.Shared.Ports.Requests.Items;
using Xipona.Frontend.Redux.Shared.Ports.Requests.Manufacturers;
using Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using Xipona.Frontend.Redux.Shared.States;
using Xipona.Frontend.Redux.ShoppingList.States;
using Xipona.Frontend.Redux.Stores.States;
using AddItemToShoppingListContract = Xipona.Api.Contracts.ShoppingLists.Commands.AddItemToShoppingList.AddItemToShoppingListContract;
using IngredientQuantityType = Xipona.Frontend.Redux.Recipes.States.IngredientQuantityType;
using ItemStore = Xipona.Frontend.Redux.Items.States.ItemStore;
using ShoppingListStore = Xipona.Frontend.Redux.ShoppingList.States.ShoppingListStore;

namespace Xipona.Frontend.Infrastructure.Connection;

public class ApiClient : IApiClient
{
    private readonly IXiponaApiClient _client;
    private readonly IApiConverters _converters;

    public ApiClient(IXiponaApiClient client, IApiConverters converters)
    {
        _client = client;
        _converters = converters;
    }

    public async Task IsAliveAsync()
    {
        _ = await _client.IsAlive();
    }

    public async Task<UserInfo> LoginAsync()
    {
        var result = await _client.LoginAsync();
        return _converters.ToDomain<UserInfoContract, UserInfo>(result);
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
            [] :
            contracts.Select(_converters.ToDomain<StoreForShoppingContract, ShoppingListStore>);
    }

    public async Task<IEnumerable<ItemStore>> GetAllActiveStoresForItemAsync()
    {
        var contracts = await _client.GetActiveStoresForItemAsync();

        return contracts is null ?
            [] :
            contracts.Select(_converters.ToDomain<StoreForItemContract, ItemStore>);
    }

    public async Task<IEnumerable<StoreSearchResult>> GetActiveStoresOverviewAsync()
    {
        var contracts = await _client.GetActiveStoresOverviewAsync();

        return contracts is null
            ? []
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
            return [];

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
            ? []
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
            [] :
            result.Select(_converters.ToDomain<QuantityTypeContract, QuantityType>);
    }

    public async Task<IEnumerable<QuantityTypeInPacket>> GetAllQuantityTypesInPacketAsync()
    {
        var result = await _client.GetAllQuantityTypesInPacketAsync();
        return result is null ?
            [] :
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
            ? []
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
            ? []
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
            ? []
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
            ? []
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
            ? []
            : _converters.ToDomain<IngredientQuantityTypeContract, IngredientQuantityType>(types);
    }

    public async Task<IEnumerable<RecipeTag>> GetAllRecipeTagsAsync()
    {
        var tags = await _client.GetAllRecipeTagsAsync();
        return tags is null
            ? []
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
            ? []
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

    public async Task AddShoppingListDiscountAsync(Guid shoppingListId, decimal discount, ShoppingListDiscountType type)
    {
        var contract = _converters
            .ToContract<(decimal, ShoppingListDiscountType), AddShoppingListDiscountContract>((discount, type));
        await _client.AddShoppingListDiscountAsync(shoppingListId, contract);
    }

    public async Task RemoveShoppingListDiscountAsync(Guid shoppingListId, Guid discountId)
    {
        await _client.RemoveShoppingListDiscountAsync(shoppingListId, discountId);
    }

    public async Task<IEnumerable<Currency>> GetAllCurrenciesAsync()
    {
        var result = await _client.GetAllCurrenciesAsync();
        return _converters.ToDomain<CurrencyContract, Currency>(result);
    }

    public async Task UpdateGeneralSettingsAsync(Currency currency)
    {
        var contract = new GeneralSettingsContract(currency.Id);
        await _client.UpdateGeneralSettingsAsync(contract);
    }

    public async Task<GeneralSettings> GetGeneralSettingsAsync()
    {
        var settings = await _client.GetGeneralSettingsAsync();
        return _converters
            .ToDomain<Api.Contracts.Users.Queries.GetGeneralSettings.GeneralSettingsContract, GeneralSettings>(settings);
    }

    public async Task<Guid> MergeItemsAsync(MergedItem mergedItem)
    {
        var contract = _converters.ToContract<MergedItem, MergeItemsContract>(mergedItem);
        var result = await _client.MergeItemsAsync(contract);
        return result;
    }

    public async Task<IEnumerable<MergeItemSearchResult>> SearchItemsForMergeAsync(EditedItem item, Guid[] alreadySelectedItems)
    {
        var contracts = await _client.SearchItemsForMergeAsync(item.ItemCategoryId!.Value, item.ManufacturerId,
            item.QuantityType.Id, item.QuantityInPacket, item.QuantityInPacketType?.Id, alreadySelectedItems);
        return contracts is null
            ? []
            : _converters.ToDomain<SearchItemsForMergeResultContract, MergeItemSearchResult>(contracts);
    }

    public async Task MarkItemAsFavoriteAsync(Guid itemId)
    {
        await _client.MarkItemAsFavoriteAsync(itemId);
    }
    
    public async Task UnmarkItemAsFavoriteAsync(Guid itemId)
    {
        await _client.UnmarkItemAsFavoriteAsync(itemId);
    }

    public async Task<List<ItemSearchResult>> FilterItemsAsync(Guid? storeId, Guid? itemCategoryId,
        Guid? manufacturerId, int page, int pageSize)
    {
        var result = await _client.FilterItemsAsync(storeId,  itemCategoryId, manufacturerId, page, pageSize);

        return [.. _converters.ToDomain<SearchItemResultContract, ItemSearchResult>(result)];
    }
}