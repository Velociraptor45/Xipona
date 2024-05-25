using Moq;
using Moq.Contrib.InOrder.Extensions;
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
using ProjectHermes.Xipona.Frontend.TestTools.Extensions;

namespace ProjectHermes.Xipona.Frontend.Redux.TestKit.Shared.Ports;

public class ApiClientMock : Mock<IApiClient>
{
    public ApiClientMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupIsAliveAsync()
    {
        this.SetupInOrder(m => m.IsAliveAsync())
            .Returns(Task.CompletedTask);
    }

    public void SetupIsAliveAsyncThrowing(Exception ex)
    {
        this.SetupInOrder(m => m.IsAliveAsync())
            .ThrowsAsync(ex);
    }

    public void SetupGetAllActiveStoresForShoppingListAsync(IEnumerable<ShoppingListStore> returnValue)
    {
        this.SetupInOrder(m => m.GetAllActiveStoresForShoppingListAsync())
            .ReturnsAsync(returnValue);
    }

    public void SetupGetAllActiveStoresForShoppingListAsyncThrowing(Exception ex)
    {
        this.SetupInOrder(m => m.GetAllActiveStoresForShoppingListAsync()).ThrowsAsync(ex);
    }

    public void SetupUpdateItemPriceAsync(UpdateItemPriceRequest request)
    {
        this.SetupInOrder(m => m.UpdateItemPriceAsync(It.Is<UpdateItemPriceRequest>(r => r.IsRequestEquivalentTo(request))))
            .Returns(Task.CompletedTask);
    }

    public void SetupUpdateItemPriceAsyncThrowing(UpdateItemPriceRequest request, Exception ex)
    {
        this.SetupInOrder(m => m.UpdateItemPriceAsync(It.Is<UpdateItemPriceRequest>(r => r.IsRequestEquivalentTo(request))))
            .ThrowsAsync(ex);
    }

    public void SetupFinishListAsync(FinishListRequest request)
    {
        this.SetupInOrder(m => m.FinishListAsync(It.Is<FinishListRequest>(r => r.IsRequestEquivalentTo(request))))
            .Returns(Task.CompletedTask);
    }

    public void SetupFinishListAsyncThrowing(FinishListRequest request, Exception ex)
    {
        this.SetupInOrder(m => m.FinishListAsync(It.Is<FinishListRequest>(r => r.IsRequestEquivalentTo(request))))
            .ThrowsAsync(ex);
    }

    public void SetupSearchItemsForShoppingListAsync(string searchInput, Guid storeId,
        IEnumerable<SearchItemForShoppingListResult> returnValue)
    {
        this.SetupInOrder(m => m.SearchItemsForShoppingListAsync(searchInput, storeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupAddItemToShoppingListAsync(AddItemToShoppingListRequest request)
    {
        this.SetupInOrder(m => m.AddItemToShoppingListAsync(It.Is<AddItemToShoppingListRequest>(r => r.IsRequestEquivalentTo(request))))
            .Returns(Task.CompletedTask);
    }

    public void VerifyAddItemToShoppingListAsync(AddItemToShoppingListRequest request, Func<Times> times)
    {
        Verify(m =>
            m.AddItemToShoppingListAsync(It.Is<AddItemToShoppingListRequest>(r => r.IsRequestEquivalentTo(request))),
            times);
    }

    public void SetupAddItemWithTypeToShoppingListAsync(AddItemWithTypeToShoppingListRequest request)
    {
        this.SetupInOrder(m => m.AddItemWithTypeToShoppingListAsync(It.Is<AddItemWithTypeToShoppingListRequest>(r => r.IsRequestEquivalentTo(request))))
            .Returns(Task.CompletedTask);
    }

    public void VerifyAddItemWithTypeToShoppingListAsync(AddItemWithTypeToShoppingListRequest request, Func<Times> times)
    {
        Verify(m =>
            m.AddItemWithTypeToShoppingListAsync(It.Is<AddItemWithTypeToShoppingListRequest>(r => r.IsRequestEquivalentTo(request))),
            times);
    }

    public void SetupGetItemByIdAsync(Guid itemId, EditedItem returnValue)
    {
        this.SetupInOrder(m => m.GetItemByIdAsync(itemId))
            .ReturnsAsync(returnValue);
    }

    public void SetupGetItemByIdAsyncThrowing(Guid itemId, Exception ex)
    {
        this.SetupInOrder(m => m.GetItemByIdAsync(itemId)).ThrowsAsync(ex);
    }

    public void SetupCreateItemAsync(EditedItem item)
    {
        this.SetupInOrder(m => m.CreateItemAsync(item))
            .Returns(Task.CompletedTask);
    }

    public void SetupCreateItemAsyncThrowing(EditedItem item, Exception ex)
    {
        this.SetupInOrder(m => m.CreateItemAsync(item))
            .ThrowsAsync(ex);
    }

    public void SetupCreateItemWithTypesAsync(EditedItem item)
    {
        this.SetupInOrder(m => m.CreateItemWithTypesAsync(item))
            .Returns(Task.CompletedTask);
    }

    public void SetupCreateItemWithTypesAsyncThrowing(EditedItem item, Exception ex)
    {
        this.SetupInOrder(m => m.CreateItemWithTypesAsync(item))
            .ThrowsAsync(ex);
    }

    public void SetupUpdateItemAsync(EditedItem item)
    {
        this.SetupInOrder(m => m.UpdateItemAsync(item))
            .Returns(Task.CompletedTask);
    }

    public void SetupUpdateItemAsyncThrowing(EditedItem item, Exception ex)
    {
        this.SetupInOrder(m => m.UpdateItemAsync(item))
            .ThrowsAsync(ex);
    }

    public void SetupUpdateItemWithTypesAsync(EditedItem item)
    {
        this.SetupInOrder(m => m.UpdateItemWithTypesAsync(item))
            .Returns(Task.CompletedTask);
    }

    public void SetupUpdateItemWithTypesAsyncThrowing(EditedItem item, Exception ex)
    {
        this.SetupInOrder(m => m.UpdateItemWithTypesAsync(item))
            .ThrowsAsync(ex);
    }

    public void SetupModifyItemAsync(EditedItem item)
    {
        this.SetupInOrder(m => m.ModifyItemAsync(item))
            .Returns(Task.CompletedTask);
    }

    public void SetupModifyItemAsyncThrowing(EditedItem item, Exception ex)
    {
        this.SetupInOrder(m => m.ModifyItemAsync(item))
            .ThrowsAsync(ex);
    }

    public void SetupModifyItemWithTypesAsync(EditedItem item)
    {
        this.SetupInOrder(m => m.ModifyItemWithTypesAsync(item))
            .Returns(Task.CompletedTask);
    }

    public void SetupModifyItemWithTypesAsyncThrowing(EditedItem item, Exception ex)
    {
        this.SetupInOrder(m => m.ModifyItemWithTypesAsync(item))
            .ThrowsAsync(ex);
    }

    public void SetupMakeTemporaryItemPermanent(MakeTemporaryItemPermanentRequest request)
    {
        this.SetupInOrder(m => m.MakeTemporaryItemPermanent(
                It.Is<MakeTemporaryItemPermanentRequest>(r => r.IsRequestEquivalentTo(request))))
            .Returns(Task.CompletedTask);
    }

    public void SetupMakeTemporaryItemPermanentThrowing(MakeTemporaryItemPermanentRequest request, Exception ex)
    {
        this.SetupInOrder(m => m.MakeTemporaryItemPermanent(
                It.Is<MakeTemporaryItemPermanentRequest>(r => r.IsRequestEquivalentTo(request))))
            .ThrowsAsync(ex);
    }

    public void SetupDeleteItemAsync(Guid itemId)
    {
        this.SetupInOrder(m => m.DeleteItemAsync(itemId))
            .Returns(Task.CompletedTask);
    }

    public void SetupDeleteItemAsyncThrowing(Guid itemId, Exception ex)
    {
        this.SetupInOrder(m => m.DeleteItemAsync(itemId))
            .ThrowsAsync(ex);
    }

    public void SetupSearchRecipesByNameAsync(string searchInput, IEnumerable<RecipeSearchResult> returnValue)
    {
        this.SetupInOrder(m => m.SearchRecipesByNameAsync(searchInput))
            .ReturnsAsync(returnValue);
    }

    public void SetupSearchRecipesByNameAsyncThrowing(string searchInput, Exception ex)
    {
        this.SetupInOrder(m => m.SearchRecipesByNameAsync(searchInput)).ThrowsAsync(ex);
    }

    public void SetupGetAllRecipeTagsAsync(IEnumerable<RecipeTag> returnValue)
    {
        this.SetupInOrder(m => m.GetAllRecipeTagsAsync())
            .ReturnsAsync(returnValue);
    }

    public void SetupGetAllRecipeTagsAsyncThrowing(Exception ex)
    {
        this.SetupInOrder(m => m.GetAllRecipeTagsAsync()).ThrowsAsync(ex);
    }

    public void SetupGetRecipeByIdAsync(Guid recipeId, EditedRecipe returnValue)
    {
        this.SetupInOrder(m => m.GetRecipeByIdAsync(recipeId))
            .ReturnsAsync(returnValue);
    }

    public void SetupGetRecipeByIdAsyncThrowing(Guid recipeId, Exception ex)
    {
        this.SetupInOrder(m => m.GetRecipeByIdAsync(recipeId)).ThrowsAsync(ex);
    }

    public void SetupModifyRecipeAsync(EditedRecipe recipe)
    {
        this.SetupInOrder(m => m.ModifyRecipeAsync(recipe))
            .Returns(Task.CompletedTask);
    }

    public void SetupModifyRecipeAsyncThrowing(EditedRecipe recipe, Exception ex)
    {
        this.SetupInOrder(m => m.ModifyRecipeAsync(recipe)).ThrowsAsync(ex);
    }

    public void SetupCreateRecipeAsync(EditedRecipe recipe, EditedRecipe returnValue)
    {
        this.SetupInOrder(m => m.CreateRecipeAsync(recipe))
            .ReturnsAsync(returnValue);
    }

    public void SetupCreateRecipeAsyncThrowing(EditedRecipe recipe, Exception ex)
    {
        this.SetupInOrder(m => m.CreateRecipeAsync(recipe)).ThrowsAsync(ex);
    }

    public void SetupCreateRecipeTagAsync(string recipeTag, RecipeTag returnValue)
    {
        this.SetupInOrder(m => m.CreateRecipeTagAsync(recipeTag))
            .ReturnsAsync(returnValue);
    }

    public void SetupCreateRecipeTagAsyncThrowing(string recipeTag, Exception ex)
    {
        this.SetupInOrder(m => m.CreateRecipeTagAsync(recipeTag)).ThrowsAsync(ex);
    }

    public void SetupSearchRecipesByTagsAsync(IEnumerable<Guid> tags,
        IEnumerable<RecipeSearchResult> returnValue)
    {
        this.SetupInOrder(m => m.SearchRecipesByTagsAsync(tags))
            .ReturnsAsync(returnValue);
    }

    public void SetupSearchRecipesByTagsAsyncThrowing(IEnumerable<Guid> tags, Exception ex)
    {
        this.SetupInOrder(m => m.SearchRecipesByTagsAsync(tags)).ThrowsAsync(ex);
    }

    public void SetupAddItemsToShoppingListsAsync(IEnumerable<AddToShoppingListItem> items)
    {
        this.SetupInOrder(m =>
                m.AddItemsToShoppingListsAsync(It.Is<IEnumerable<AddToShoppingListItem>>(itms => itms.IsEquivalentTo(items))))
            .Returns(Task.CompletedTask);
    }

    public void SetupAddItemsToShoppingListsAsyncThrowing(IEnumerable<AddToShoppingListItem> items, Exception ex)
    {
        this.SetupInOrder(m =>
                m.AddItemsToShoppingListsAsync(It.Is<IEnumerable<AddToShoppingListItem>>(itms => itms.IsEquivalentTo(items))))
            .ThrowsAsync(ex);
    }

    public void SetupGetItemAmountsForOneServingAsync(Guid recipeId, IEnumerable<AddToShoppingListItem> returnValue)
    {
        this.SetupInOrder(m => m.GetItemAmountsForOneServingAsync(recipeId))
            .ReturnsAsync(returnValue);
    }

    public void SetupGetItemAmountsForOneServingAsyncThrowing(Guid recipeId, Exception ex)
    {
        this.SetupInOrder(m => m.GetItemAmountsForOneServingAsync(recipeId)).ThrowsAsync(ex);
    }

    public void SetupGetAllQuantityTypesAsync(IEnumerable<QuantityType> returnValue)
    {
        this.SetupInOrder(m => m.GetAllQuantityTypesAsync())
            .ReturnsAsync(returnValue);
    }

    public void SetupGetAllQuantityTypesAsyncThrowing(Exception ex)
    {
        this.SetupInOrder(m => m.GetAllQuantityTypesAsync()).ThrowsAsync(ex);
    }

    public void SetupGetAllQuantityTypesInPacketAsync(IEnumerable<QuantityTypeInPacket> returnValue)
    {
        this.SetupInOrder(m => m.GetAllQuantityTypesInPacketAsync())
            .ReturnsAsync(returnValue);
    }

    public void SetupGetAllQuantityTypesInPacketAsyncThrowing(Exception ex)
    {
        this.SetupInOrder(m => m.GetAllQuantityTypesInPacketAsync()).ThrowsAsync(ex);
    }

    public void SetupGetActiveShoppingListByStoreIdAsync(Guid storeId, ShoppingListModel returnValue)
    {
        this.SetupInOrder(m => m.GetActiveShoppingListByStoreIdAsync(storeId))
            .ReturnsAsync(returnValue);
    }

    public void SetupGetActiveShoppingListByStoreIdAsyncThrowing(Guid storeId, Exception ex)
    {
        this.SetupInOrder(m => m.GetActiveShoppingListByStoreIdAsync(storeId)).ThrowsAsync(ex);
    }

    public void SetupGetStoreByIdAsync(Guid storeId, EditedStore returnValue)
    {
        this.SetupInOrder(m => m.GetStoreByIdAsync(storeId)).ReturnsAsync(returnValue);
    }

    public void SetupGetStoreByIdAsyncThrowing(Guid storeId, Exception ex)
    {
        this.SetupInOrder(m => m.GetStoreByIdAsync(storeId)).ThrowsAsync(ex);
    }

    public void SetupCreateStoreAsync(EditedStore store)
    {
        this.SetupInOrder(m => m.CreateStoreAsync(It.Is<EditedStore>(s => s.IsEquivalentTo(store))))
            .Returns(Task.CompletedTask);
    }

    public void SetupCreateStoreAsyncThrowing(EditedStore store, Exception ex)
    {
        this.SetupInOrder(m => m.CreateStoreAsync(It.Is<EditedStore>(s => s.IsEquivalentTo(store))))
            .ThrowsAsync(ex);
    }

    public void SetupModifyStoreAsync(EditedStore store)
    {
        this.SetupInOrder(m => m.ModifyStoreAsync(store)).Returns(Task.CompletedTask);
    }

    public void SetupModifyStoreAsyncThrowing(EditedStore store, Exception ex)
    {
        this.SetupInOrder(m => m.ModifyStoreAsync(store)).ThrowsAsync(ex);
    }

    public void SetupDeleteStoreAsync(Guid storeId)
    {
        this.SetupInOrder(m => m.DeleteStoreAsync(storeId)).Returns(Task.CompletedTask);
    }

    public void SetupDeleteStoreAsyncThrowing(Guid storeId, Exception ex)
    {
        this.SetupInOrder(m => m.DeleteStoreAsync(storeId)).ThrowsAsync(ex);
    }

    public void SetupGetAllIngredientQuantityTypes(IEnumerable<IngredientQuantityType> returnValue)
    {
        this.SetupInOrder(m => m.GetAllIngredientQuantityTypes()).ReturnsAsync(returnValue);
    }

    public void SetupGetAllIngredientQuantityTypesThrowing(Exception ex)
    {
        this.SetupInOrder(m => m.GetAllIngredientQuantityTypes()).ThrowsAsync(ex);
    }

    public void SetupGetManufacturerSearchResultsAsync(string searchInput,
        IEnumerable<ManufacturerSearchResult> returnValue)
    {
        this.SetupInOrder(m => m.GetManufacturerSearchResultsAsync(searchInput))
            .ReturnsAsync(returnValue);
    }

    public void SetupGetManufacturerSearchResultsAsyncThrowing(string searchInput, Exception ex)
    {
        this.SetupInOrder(m => m.GetManufacturerSearchResultsAsync(searchInput)).ThrowsAsync(ex);
    }

    public void SetupGetManufacturerByIdAsync(Guid manufacturerId, EditedManufacturer returnValue)
    {
        this.SetupInOrder(m => m.GetManufacturerByIdAsync(manufacturerId))
            .ReturnsAsync(returnValue);
    }

    public void SetupGetManufacturerByIdAsyncThrowing(Guid manufacturerId, Exception ex)
    {
        this.SetupInOrder(m => m.GetManufacturerByIdAsync(manufacturerId)).ThrowsAsync(ex);
    }

    public void SetupCreateManufacturerAsync(string name, EditedManufacturer returnValue)
    {
        this.SetupInOrder(m => m.CreateManufacturerAsync(name)).ReturnsAsync(returnValue);
    }

    public void SetupCreateManufacturerAsyncThrowing(string name, Exception ex)
    {
        this.SetupInOrder(m => m.CreateManufacturerAsync(name)).ThrowsAsync(ex);
    }

    public void SetupModifyManufacturerAsync(ModifyManufacturerRequest request)
    {
        this.SetupInOrder(m => m.ModifyManufacturerAsync(
                It.Is<ModifyManufacturerRequest>(r => r.IsRequestEquivalentTo(request))))
            .Returns(Task.CompletedTask);
    }

    public void SetupModifyManufacturerAsyncThrowing(ModifyManufacturerRequest request, Exception ex)
    {
        this.SetupInOrder(m => m.ModifyManufacturerAsync(
                It.Is<ModifyManufacturerRequest>(r => r.IsRequestEquivalentTo(request))))
            .ThrowsAsync(ex);
    }

    public void SetupDeleteManufacturerAsync(Guid manufacturerId)
    {
        this.SetupInOrder(m => m.DeleteManufacturerAsync(manufacturerId)).Returns(Task.CompletedTask);
    }

    public void SetupDeleteManufacturerAsyncThrowing(Guid manufacturerId, Exception ex)
    {
        this.SetupInOrder(m => m.DeleteManufacturerAsync(manufacturerId)).ThrowsAsync(ex);
    }

    public void SetupGetItemCategorySearchResultsAsync(string searchInput,
        IEnumerable<ItemCategorySearchResult> returnValue)
    {
        this.SetupInOrder(m => m.GetItemCategorySearchResultsAsync(searchInput))
            .ReturnsAsync(returnValue);
    }

    public void SetupGetItemCategorySearchResultsAsyncThrowing(string searchInput, Exception ex)
    {
        this.SetupInOrder(m => m.GetItemCategorySearchResultsAsync(searchInput)).ThrowsAsync(ex);
    }

    public void SetupGetItemCategoryByIdAsync(Guid manufacturerId, EditedItemCategory returnValue)
    {
        this.SetupInOrder(m => m.GetItemCategoryByIdAsync(manufacturerId))
            .ReturnsAsync(returnValue);
    }

    public void SetupGetItemCategoryByIdAsyncThrowing(Guid manufacturerId, Exception ex)
    {
        this.SetupInOrder(m => m.GetItemCategoryByIdAsync(manufacturerId)).ThrowsAsync(ex);
    }

    public void SetupCreateItemCategoryAsync(string name, EditedItemCategory returnValue)
    {
        this.SetupInOrder(m => m.CreateItemCategoryAsync(name)).ReturnsAsync(returnValue);
    }

    public void SetupCreateItemCategoryAsyncThrowing(string name, Exception ex)
    {
        this.SetupInOrder(m => m.CreateItemCategoryAsync(name)).ThrowsAsync(ex);
    }

    public void SetupModifyItemCategoryAsync(ModifyItemCategoryRequest request)
    {
        this.SetupInOrder(m => m.ModifyItemCategoryAsync(
                It.Is<ModifyItemCategoryRequest>(r => r.IsRequestEquivalentTo(request))))
            .Returns(Task.CompletedTask);
    }

    public void SetupModifyItemCategoryAsyncThrowing(ModifyItemCategoryRequest request, Exception ex)
    {
        this.SetupInOrder(m => m.ModifyItemCategoryAsync(
                It.Is<ModifyItemCategoryRequest>(r => r.IsRequestEquivalentTo(request))))
            .ThrowsAsync(ex);
    }

    public void SetupDeleteItemCategoryAsync(Guid manufacturerId)
    {
        this.SetupInOrder(m => m.DeleteItemCategoryAsync(manufacturerId)).Returns(Task.CompletedTask);
    }

    public void SetupDeleteItemCategoryAsyncThrowing(Guid manufacturerId, Exception ex)
    {
        this.SetupInOrder(m => m.DeleteItemCategoryAsync(manufacturerId)).ThrowsAsync(ex);
    }

    public void SetupSearchItemsAsync(string searchInput, int page, int pageSize,
        IEnumerable<ItemSearchResult> returnValue)
    {
        this.SetupInOrder(m => m.SearchItemsAsync(searchInput, page, pageSize))
            .ReturnsAsync(returnValue);
    }

    public void SetupSearchItemsAsyncThrowing(string searchInput, int page, int pageSize, Exception ex)
    {
        this.SetupInOrder(m => m.SearchItemsAsync(searchInput, page, pageSize)).ThrowsAsync(ex);
    }

    public void SetupGetAllActiveStoresForItemAsync(IEnumerable<ItemStore> returnValue)
    {
        this.SetupInOrder(m => m.GetAllActiveStoresForItemAsync())
            .ReturnsAsync(returnValue);
    }

    public void SetupGetAllActiveStoresForItemAsyncThrowing(Exception ex)
    {
        this.SetupInOrder(m => m.GetAllActiveStoresForItemAsync()).ThrowsAsync(ex);
    }

    public void SetupGetTotalSearchResultCountAsync(string searchInput, int returnValue)
    {
        this.SetupInOrder(m => m.GetTotalSearchResultCountAsync(searchInput))
            .ReturnsAsync(returnValue);
    }

    public void SetupGetTotalSearchResultCountAsyncThrowing(string searchInput, Exception ex)
    {
        this.SetupInOrder(m => m.GetTotalSearchResultCountAsync(searchInput)).ThrowsAsync(ex);
    }
}