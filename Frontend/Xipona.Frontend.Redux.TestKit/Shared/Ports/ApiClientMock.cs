using Moq;
using Moq.Contrib.InOrder;
using Moq.Contrib.InOrder.Extensions;
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
using Xipona.Frontend.TestTools.Extensions;

namespace Xipona.Frontend.Redux.TestKit.Shared.Ports;

public class ApiClientMock : Mock<IApiClient>
{
    public ApiClientMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupIsAliveAsync(IQueueComponent component)
    {
        this.SetupInOrder(m => m.IsAliveAsync(), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupIsAliveAsyncThrowing(Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.IsAliveAsync(), component)
            .ThrowsAsync(ex);
    }

    public void SetupGetAllActiveStoresForShoppingListAsync(IEnumerable<ShoppingListStore> returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetAllActiveStoresForShoppingListAsync(), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupGetAllActiveStoresForShoppingListAsyncThrowing(Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetAllActiveStoresForShoppingListAsync(), component).ThrowsAsync(ex);
    }

    public void SetupUpdateItemPriceAsync(UpdateItemPriceRequest request, IQueueComponent component)
    {
        this.SetupInOrder(m => m.UpdateItemPriceAsync(It.Is<UpdateItemPriceRequest>(r => r.IsRequestEquivalentTo(request))), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupUpdateItemPriceAsyncThrowing(UpdateItemPriceRequest request, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.UpdateItemPriceAsync(It.Is<UpdateItemPriceRequest>(r => r.IsRequestEquivalentTo(request))), component)
            .ThrowsAsync(ex);
    }

    public void SetupFinishListAsync(FinishListRequest request, IQueueComponent component)
    {
        this.SetupInOrder(m => m.FinishListAsync(It.Is<FinishListRequest>(r => r.IsRequestEquivalentTo(request))), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupFinishListAsyncThrowing(FinishListRequest request, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.FinishListAsync(It.Is<FinishListRequest>(r => r.IsRequestEquivalentTo(request))), component)
            .ThrowsAsync(ex);
    }

    public void SetupSearchItemsForShoppingListAsync(string searchInput, Guid storeId,
        IEnumerable<SearchItemForShoppingListResult> returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.SearchItemsForShoppingListAsync(searchInput, storeId, It.IsAny<CancellationToken>()), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupAddItemToShoppingListAsync(AddItemToShoppingListRequest request, IQueueComponent component)
    {
        this.SetupInOrder(m => m.AddItemToShoppingListAsync(It.Is<AddItemToShoppingListRequest>(r => r.IsRequestEquivalentTo(request))),
                component)
            .Returns(Task.CompletedTask);
    }

    public void VerifyAddItemToShoppingListAsync(AddItemToShoppingListRequest request, Func<Times> times)
    {
        Verify(m =>
            m.AddItemToShoppingListAsync(It.Is<AddItemToShoppingListRequest>(r => r.IsRequestEquivalentTo(request))),
            times);
    }

    public void SetupAddItemWithTypeToShoppingListAsync(AddItemWithTypeToShoppingListRequest request, IQueueComponent component)
    {
        this.SetupInOrder(m => m.AddItemWithTypeToShoppingListAsync(It.Is<AddItemWithTypeToShoppingListRequest>(r => r.IsRequestEquivalentTo(request))),
                component)
            .Returns(Task.CompletedTask);
    }

    public void VerifyAddItemWithTypeToShoppingListAsync(AddItemWithTypeToShoppingListRequest request, Func<Times> times)
    {
        Verify(m =>
            m.AddItemWithTypeToShoppingListAsync(It.Is<AddItemWithTypeToShoppingListRequest>(r => r.IsRequestEquivalentTo(request))),
            times);
    }

    public void SetupGetItemByIdAsync(Guid itemId, EditedItem returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetItemByIdAsync(itemId), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupGetItemByIdAsyncThrowing(Guid itemId, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetItemByIdAsync(itemId), component).ThrowsAsync(ex);
    }

    public void SetupCreateItemAsync(EditedItem item, IQueueComponent component)
    {
        this.SetupInOrder(m => m.CreateItemAsync(item), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupCreateItemAsyncThrowing(EditedItem item, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.CreateItemAsync(item), component)
            .ThrowsAsync(ex);
    }

    public void SetupCreateItemWithTypesAsync(EditedItem item, IQueueComponent component)
    {
        this.SetupInOrder(m => m.CreateItemWithTypesAsync(item), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupCreateItemWithTypesAsyncThrowing(EditedItem item, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.CreateItemWithTypesAsync(item), component)
            .ThrowsAsync(ex);
    }

    public void SetupUpdateItemAsync(EditedItem item, IQueueComponent component)
    {
        this.SetupInOrder(m => m.UpdateItemAsync(item), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupUpdateItemAsyncThrowing(EditedItem item, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.UpdateItemAsync(item), component)
            .ThrowsAsync(ex);
    }

    public void SetupUpdateItemWithTypesAsync(EditedItem item, IQueueComponent component)
    {
        this.SetupInOrder(m => m.UpdateItemWithTypesAsync(item), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupUpdateItemWithTypesAsyncThrowing(EditedItem item, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.UpdateItemWithTypesAsync(item), component)
            .ThrowsAsync(ex);
    }

    public void SetupModifyItemAsync(EditedItem item, IQueueComponent component)
    {
        this.SetupInOrder(m => m.ModifyItemAsync(item), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupModifyItemAsyncThrowing(EditedItem item, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.ModifyItemAsync(item), component)
            .ThrowsAsync(ex);
    }

    public void SetupModifyItemWithTypesAsync(EditedItem item, IQueueComponent component)
    {
        this.SetupInOrder(m => m.ModifyItemWithTypesAsync(item), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupModifyItemWithTypesAsyncThrowing(EditedItem item, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.ModifyItemWithTypesAsync(item), component)
            .ThrowsAsync(ex);
    }

    public void SetupMakeTemporaryItemPermanent(MakeTemporaryItemPermanentRequest request, IQueueComponent component)
    {
        this.SetupInOrder(m => m.MakeTemporaryItemPermanent(
                It.Is<MakeTemporaryItemPermanentRequest>(r => r.IsRequestEquivalentTo(request))),
                component)
            .Returns(Task.CompletedTask);
    }

    public void SetupMakeTemporaryItemPermanentThrowing(MakeTemporaryItemPermanentRequest request, Exception ex,
        IQueueComponent component)
    {
        this.SetupInOrder(m => m.MakeTemporaryItemPermanent(
                It.Is<MakeTemporaryItemPermanentRequest>(r => r.IsRequestEquivalentTo(request))),
                component)
            .ThrowsAsync(ex);
    }

    public void SetupDeleteItemAsync(Guid itemId, IQueueComponent component)
    {
        this.SetupInOrder(m => m.DeleteItemAsync(itemId), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupDeleteItemAsyncThrowing(Guid itemId, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.DeleteItemAsync(itemId), component)
            .ThrowsAsync(ex);
    }

    public void SetupSearchRecipesByNameAsync(string searchInput, IEnumerable<RecipeSearchResult> returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.SearchRecipesByNameAsync(searchInput), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupSearchRecipesByNameAsyncThrowing(string searchInput, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.SearchRecipesByNameAsync(searchInput), component).ThrowsAsync(ex);
    }

    public void SetupGetAllRecipeTagsAsync(IEnumerable<RecipeTag> returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetAllRecipeTagsAsync(), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupGetAllRecipeTagsAsyncThrowing(Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetAllRecipeTagsAsync(), component).ThrowsAsync(ex);
    }

    public void SetupGetRecipeByIdAsync(Guid recipeId, EditedRecipe returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetRecipeByIdAsync(recipeId), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupGetRecipeByIdAsyncThrowing(Guid recipeId, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetRecipeByIdAsync(recipeId), component).ThrowsAsync(ex);
    }

    public void SetupModifyRecipeAsync(EditedRecipe recipe, IQueueComponent component)
    {
        this.SetupInOrder(m => m.ModifyRecipeAsync(recipe), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupModifyRecipeAsyncThrowing(EditedRecipe recipe, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.ModifyRecipeAsync(recipe), component).ThrowsAsync(ex);
    }

    public void SetupCreateRecipeAsync(EditedRecipe recipe, EditedRecipe returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.CreateRecipeAsync(recipe), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupCreateRecipeAsyncThrowing(EditedRecipe recipe, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.CreateRecipeAsync(recipe), component).ThrowsAsync(ex);
    }

    public void SetupCreateRecipeTagAsync(string recipeTag, RecipeTag returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.CreateRecipeTagAsync(recipeTag), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupCreateRecipeTagAsyncThrowing(string recipeTag, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.CreateRecipeTagAsync(recipeTag), component).ThrowsAsync(ex);
    }

    public void SetupSearchRecipesByTagsAsync(IEnumerable<Guid> tags,
        IEnumerable<RecipeSearchResult> returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.SearchRecipesByTagsAsync(tags), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupSearchRecipesByTagsAsyncThrowing(IEnumerable<Guid> tags, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.SearchRecipesByTagsAsync(tags), component).ThrowsAsync(ex);
    }

    public void SetupAddItemsToShoppingListsAsync(IEnumerable<AddToShoppingListItem> items, IQueueComponent component)
    {
        this.SetupInOrder(m =>
                m.AddItemsToShoppingListsAsync(It.Is<IEnumerable<AddToShoppingListItem>>(itms => itms.IsEquivalentTo(items))),
                component)
            .Returns(Task.CompletedTask);
    }

    public void SetupAddItemsToShoppingListsAsyncThrowing(IEnumerable<AddToShoppingListItem> items, Exception ex,
        IQueueComponent component)
    {
        this.SetupInOrder(m =>
                m.AddItemsToShoppingListsAsync(It.Is<IEnumerable<AddToShoppingListItem>>(itms => itms.IsEquivalentTo(items))),
                component)
            .ThrowsAsync(ex);
    }

    public void SetupGetItemAmountsForOneServingAsync(Guid recipeId, IEnumerable<AddToShoppingListItem> returnValue,
        IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetItemAmountsForOneServingAsync(recipeId), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupGetItemAmountsForOneServingAsyncThrowing(Guid recipeId, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetItemAmountsForOneServingAsync(recipeId), component).ThrowsAsync(ex);
    }

    public void SetupGetAllQuantityTypesAsync(IEnumerable<QuantityType> returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetAllQuantityTypesAsync(), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupGetAllQuantityTypesAsyncThrowing(Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetAllQuantityTypesAsync(), component).ThrowsAsync(ex);
    }

    public void SetupGetAllQuantityTypesInPacketAsync(IEnumerable<QuantityTypeInPacket> returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetAllQuantityTypesInPacketAsync(), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupGetAllQuantityTypesInPacketAsyncThrowing(Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetAllQuantityTypesInPacketAsync(), component).ThrowsAsync(ex);
    }

    public void SetupGetActiveShoppingListByStoreIdAsync(Guid storeId, ShoppingListModel returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetActiveShoppingListByStoreIdAsync(storeId), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupGetActiveShoppingListByStoreIdAsyncThrowing(Guid storeId, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetActiveShoppingListByStoreIdAsync(storeId), component).ThrowsAsync(ex);
    }

    public void SetupGetStoreByIdAsync(Guid storeId, EditedStore returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetStoreByIdAsync(storeId), component).ReturnsAsync(returnValue);
    }

    public void SetupGetStoreByIdAsyncThrowing(Guid storeId, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetStoreByIdAsync(storeId), component).ThrowsAsync(ex);
    }

    public void SetupCreateStoreAsync(EditedStore store, IQueueComponent component)
    {
        this.SetupInOrder(m => m.CreateStoreAsync(It.Is<EditedStore>(s => s.IsEquivalentTo(store))), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupCreateStoreAsyncThrowing(EditedStore store, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.CreateStoreAsync(It.Is<EditedStore>(s => s.IsEquivalentTo(store))), component)
            .ThrowsAsync(ex);
    }

    public void SetupModifyStoreAsync(EditedStore store, IQueueComponent component)
    {
        this.SetupInOrder(m => m.ModifyStoreAsync(store), component).Returns(Task.CompletedTask);
    }

    public void SetupModifyStoreAsyncThrowing(EditedStore store, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.ModifyStoreAsync(store), component).ThrowsAsync(ex);
    }

    public void SetupDeleteStoreAsync(Guid storeId, IQueueComponent component)
    {
        this.SetupInOrder(m => m.DeleteStoreAsync(storeId), component).Returns(Task.CompletedTask);
    }

    public void SetupDeleteStoreAsyncThrowing(Guid storeId, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.DeleteStoreAsync(storeId), component).ThrowsAsync(ex);
    }

    public void SetupGetAllIngredientQuantityTypes(IEnumerable<IngredientQuantityType> returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetAllIngredientQuantityTypes(), component).ReturnsAsync(returnValue);
    }

    public void SetupGetAllIngredientQuantityTypesThrowing(Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetAllIngredientQuantityTypes(), component).ThrowsAsync(ex);
    }

    public void SetupGetManufacturerSearchResultsAsync(string searchInput,
        IEnumerable<ManufacturerSearchResult> returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetManufacturerSearchResultsAsync(searchInput), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupGetManufacturerSearchResultsAsyncThrowing(string searchInput, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetManufacturerSearchResultsAsync(searchInput), component).ThrowsAsync(ex);
    }

    public void SetupGetManufacturerByIdAsync(Guid manufacturerId, EditedManufacturer returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetManufacturerByIdAsync(manufacturerId), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupGetManufacturerByIdAsyncThrowing(Guid manufacturerId, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetManufacturerByIdAsync(manufacturerId), component).ThrowsAsync(ex);
    }

    public void SetupCreateManufacturerAsync(string name, EditedManufacturer returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.CreateManufacturerAsync(name), component).ReturnsAsync(returnValue);
    }

    public void SetupCreateManufacturerAsyncThrowing(string name, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.CreateManufacturerAsync(name), component).ThrowsAsync(ex);
    }

    public void SetupModifyManufacturerAsync(ModifyManufacturerRequest request, IQueueComponent component)
    {
        this.SetupInOrder(m => m.ModifyManufacturerAsync(
                It.Is<ModifyManufacturerRequest>(r => r.IsRequestEquivalentTo(request))), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupModifyManufacturerAsyncThrowing(ModifyManufacturerRequest request, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.ModifyManufacturerAsync(
                It.Is<ModifyManufacturerRequest>(r => r.IsRequestEquivalentTo(request))), component)
            .ThrowsAsync(ex);
    }

    public void SetupDeleteManufacturerAsync(Guid manufacturerId, IQueueComponent component)
    {
        this.SetupInOrder(m => m.DeleteManufacturerAsync(manufacturerId), component).Returns(Task.CompletedTask);
    }

    public void SetupDeleteManufacturerAsyncThrowing(Guid manufacturerId, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.DeleteManufacturerAsync(manufacturerId), component).ThrowsAsync(ex);
    }

    public void SetupGetItemCategorySearchResultsAsync(string searchInput,
        IEnumerable<ItemCategorySearchResult> returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetItemCategorySearchResultsAsync(searchInput), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupGetItemCategorySearchResultsAsyncThrowing(string searchInput, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetItemCategorySearchResultsAsync(searchInput), component).ThrowsAsync(ex);
    }

    public void SetupGetItemCategoryByIdAsync(Guid itemCategoryId, EditedItemCategory returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetItemCategoryByIdAsync(itemCategoryId), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupGetItemCategoryByIdAsyncThrowing(Guid itemCategoryId, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetItemCategoryByIdAsync(itemCategoryId), component).ThrowsAsync(ex);
    }

    public void SetupCreateItemCategoryAsync(string name, EditedItemCategory returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.CreateItemCategoryAsync(name), component).ReturnsAsync(returnValue);
    }

    public void SetupCreateItemCategoryAsyncThrowing(string name, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.CreateItemCategoryAsync(name), component).ThrowsAsync(ex);
    }

    public void SetupModifyItemCategoryAsync(ModifyItemCategoryRequest request, IQueueComponent component)
    {
        this.SetupInOrder(m => m.ModifyItemCategoryAsync(
                It.Is<ModifyItemCategoryRequest>(r => r.IsRequestEquivalentTo(request))), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupModifyItemCategoryAsyncThrowing(ModifyItemCategoryRequest request, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.ModifyItemCategoryAsync(
                It.Is<ModifyItemCategoryRequest>(r => r.IsRequestEquivalentTo(request))), component)
            .ThrowsAsync(ex);
    }

    public void SetupDeleteItemCategoryAsync(Guid manufacturerId, IQueueComponent component)
    {
        this.SetupInOrder(m => m.DeleteItemCategoryAsync(manufacturerId), component).Returns(Task.CompletedTask);
    }

    public void SetupDeleteItemCategoryAsyncThrowing(Guid manufacturerId, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.DeleteItemCategoryAsync(manufacturerId), component).ThrowsAsync(ex);
    }

    public void SetupSearchItemsAsync(string searchInput, int page, int pageSize,
        IEnumerable<ItemSearchResult> returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.SearchItemsAsync(searchInput, page, pageSize), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupSearchItemsAsyncThrowing(string searchInput, int page, int pageSize, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.SearchItemsAsync(searchInput, page, pageSize), component).ThrowsAsync(ex);
    }

    public void SetupGetAllActiveStoresForItemAsync(IEnumerable<ItemStore> returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetAllActiveStoresForItemAsync(), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupGetAllActiveStoresForItemAsyncThrowing(Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetAllActiveStoresForItemAsync(), component).ThrowsAsync(ex);
    }

    public void SetupGetTotalSearchResultCountAsync(string searchInput, int returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetTotalSearchResultCountAsync(searchInput), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupGetTotalSearchResultCountAsyncThrowing(string searchInput, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetTotalSearchResultCountAsync(searchInput), component).ThrowsAsync(ex);
    }

    public void SetupSearchItemByItemCategoryAsync(Guid itemCategoryId, IEnumerable<SearchItemByItemCategoryResult> returnValue,
        IQueueComponent component)
    {
        this.SetupInOrder(m => m.SearchItemByItemCategoryAsync(itemCategoryId), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupSearchItemByItemCategoryAsyncThrowing(Guid itemCategoryId, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.SearchItemByItemCategoryAsync(itemCategoryId), component).ThrowsAsync(ex);
    }

    public void SetupGetItemTypePricesAsync(Guid itemId, Guid storeId, IEnumerable<ItemTypePrice> returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetItemTypePricesAsync(itemId, storeId), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupGetItemTypePricesAsyncThrowing(Guid itemId, Guid storeId, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetItemTypePricesAsync(itemId, storeId), component).ThrowsAsync(ex);
    }

    public void SetupAddItemDiscountAsync(Guid shoppingListId, Guid itemId, Guid? itemTypeId, decimal discount,
        IQueueComponent component)
    {
        this.SetupInOrder(m => m.AddItemDiscountAsync(shoppingListId, itemId, itemTypeId, discount), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupAddItemDiscountAsyncThrowing(Guid shoppingListId, Guid itemId, Guid? itemTypeId, decimal discount,
        Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.AddItemDiscountAsync(shoppingListId, itemId, itemTypeId, discount), component)
            .ThrowsAsync(ex);
    }

    public void SetupRemoveItemDiscountAsync(Guid shoppingListId, Guid itemId, Guid? itemTypeId, IQueueComponent component)
    {
        this.SetupInOrder(m => m.RemoveItemDiscountAsync(shoppingListId, itemId, itemTypeId), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupRemoveItemDiscountAsyncThrowing(Guid shoppingListId, Guid itemId, Guid? itemTypeId, Exception ex,
        IQueueComponent component)
    {
        this.SetupInOrder(m => m.RemoveItemDiscountAsync(shoppingListId, itemId, itemTypeId), component)
            .ThrowsAsync(ex);
    }

    public void SetupGetGeneralSettingsAsync(GeneralSettings returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetGeneralSettingsAsync(), component).ReturnsAsync(returnValue);
    }

    public void SetupGetGeneralSettingsAsyncThrowing(Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetGeneralSettingsAsync(), component).ThrowsAsync(ex);
    }

    public void SetupGetAllCurrenciesAsync(IEnumerable<Currency> returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetAllCurrenciesAsync(), component).ReturnsAsync(returnValue);
    }

    public void SetupGetAllCurrenciesAsyncThrowing(Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.GetAllCurrenciesAsync(), component).ThrowsAsync(ex);
    }

    public void SetupUpdateGeneralSettingsAsync(Currency currency, IQueueComponent component)
    {
        this.SetupInOrder(m => m.UpdateGeneralSettingsAsync(currency), component).Returns(Task.CompletedTask);
    }

    public void SetupUpdateGeneralSettingsAsyncThrowing(Currency currency, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.UpdateGeneralSettingsAsync(currency), component).ThrowsAsync(ex);
    }

    public void SetupAddShoppingListDiscountAsync(Guid shoppingListId, decimal discount, ShoppingListDiscountType type,
        IQueueComponent component)
    {
        this.SetupInOrder(m => m.AddShoppingListDiscountAsync(shoppingListId, discount, type), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupAddShoppingListDiscountAsyncThrowing(Guid shoppingListId, decimal discount,
        ShoppingListDiscountType type, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.AddShoppingListDiscountAsync(shoppingListId, discount, type), component)
            .ThrowsAsync(ex);
    }

    public void SetupRemoveShoppingListDiscountAsync(Guid shoppingListId, Guid discountId, IQueueComponent component)
    {
        this.SetupInOrder(m => m.RemoveShoppingListDiscountAsync(shoppingListId, discountId), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupRemoveShoppingListDiscountAsyncThrowing(Guid shoppingListId, Guid discountId, Exception ex,
        IQueueComponent component)
    {
        this.SetupInOrder(m => m.RemoveShoppingListDiscountAsync(shoppingListId, discountId), component)
            .ThrowsAsync(ex);
    }

    public void SetupMergeItemsAsync(MergedItem item, Guid returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.MergeItemsAsync(It.Is<MergedItem>(i => i.IsEquivalentTo(item))), component)
            .ReturnsAsync(returnValue);
    }

    public void SetupMergeItemsAsyncThrowing(MergedItem item, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(m => m.MergeItemsAsync(It.Is<MergedItem>(i => i.IsEquivalentTo(item))), component)
            .ThrowsAsync(ex);
    }

    public void SetupSearchItemsForMergeAsync(EditedItem item, Guid[] alreadySelectedItems,
        IEnumerable<MergeItemSearchResult> returnValue, IQueueComponent component)
    {
        this.SetupInOrder(m => m.SearchItemsForMergeAsync(
                It.Is<EditedItem>(i => i.IsEquivalentTo(item)),
                It.Is<Guid[]>(ids => ids.IsEquivalentTo(alreadySelectedItems))),
                component)
            .ReturnsAsync(returnValue);
    }

    public void SetupSearchItemsForMergeAsyncThrowing(EditedItem item, Guid[] alreadySelectedItems, Exception ex,
        IQueueComponent component)
    {
        this.SetupInOrder(m => m.SearchItemsForMergeAsync(
                It.Is<EditedItem>(i => i.IsEquivalentTo(item)),
                It.Is<Guid[]>(ids => ids.IsEquivalentTo(alreadySelectedItems))),
                component)
            .ThrowsAsync(ex);
    }
}