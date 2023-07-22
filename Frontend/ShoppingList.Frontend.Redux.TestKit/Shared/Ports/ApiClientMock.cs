using Moq;
using Moq.Contrib.InOrder.Extensions;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;
using ProjectHermes.ShoppingList.Frontend.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Ports;

public class ApiClientMock : Mock<IApiClient>
{
    public ApiClientMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupGetAllActiveStoresForShoppingListAsync(IEnumerable<ShoppingListStore> returnValue)
    {
        this.SetupInOrder(m => m.GetAllActiveStoresForShoppingListAsync())
            .ReturnsAsync(returnValue);
    }

    public void SetupUpdateItemPriceAsync(UpdateItemPriceRequest request)
    {
        this.SetupInOrder(m => m.UpdateItemPriceAsync(It.Is<UpdateItemPriceRequest>(r => r.IsRequestEquivalentTo(request))))
            .Returns(Task.CompletedTask);
    }

    public void VerifyUpdateItemPriceAsync(UpdateItemPriceRequest request, Func<Times> times)
    {
        Verify(m => m.UpdateItemPriceAsync(It.Is<UpdateItemPriceRequest>(r => r.IsRequestEquivalentTo(request))), times);
    }

    public void SetupFinishListAsync(FinishListRequest request)
    {
        this.SetupInOrder(m => m.FinishListAsync(It.Is<FinishListRequest>(r => r.IsRequestEquivalentTo(request))))
            .Returns(Task.CompletedTask);
    }

    public void VerifyFinishListAsync(FinishListRequest request, Func<Times> times)
    {
        Verify(m => m.FinishListAsync(It.Is<FinishListRequest>(r => r.IsRequestEquivalentTo(request))), times);
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

    public void VerifyGetItemByIdAsync(Guid itemId, Func<Times> times)
    {
        Verify(m => m.GetItemByIdAsync(itemId), times);
    }

    public void SetupCreateItemAsync(CreateItemRequest request)
    {
        this.SetupInOrder(m => m.CreateItemAsync(It.Is<CreateItemRequest>(r => r.IsRequestEquivalentTo(request))))
            .Returns(Task.CompletedTask);
    }

    public void SetupCreateItemAsyncThrowing(CreateItemRequest request, Exception ex)
    {
        this.SetupInOrder(m => m.CreateItemAsync(It.Is<CreateItemRequest>(r => r.IsRequestEquivalentTo(request))))
            .ThrowsAsync(ex);
    }

    public void SetupCreateItemWithTypesAsync(CreateItemWithTypesRequest request)
    {
        this.SetupInOrder(m => m.CreateItemWithTypesAsync(
                It.Is<CreateItemWithTypesRequest>(r => r.IsRequestEquivalentTo(request))))
            .Returns(Task.CompletedTask);
    }

    public void SetupCreateItemWithTypesAsyncThrowing(CreateItemWithTypesRequest request, Exception ex)
    {
        this.SetupInOrder(m => m.CreateItemWithTypesAsync(
                It.Is<CreateItemWithTypesRequest>(r => r.IsRequestEquivalentTo(request))))
            .ThrowsAsync(ex);
    }

    public void SetupUpdateItemAsync(UpdateItemRequest request)
    {
        this.SetupInOrder(m => m.UpdateItemAsync(It.Is<UpdateItemRequest>(r => r.IsRequestEquivalentTo(request))))
            .Returns(Task.CompletedTask);
    }

    public void SetupUpdateItemAsyncThrowing(UpdateItemRequest request, Exception ex)
    {
        this.SetupInOrder(m => m.UpdateItemAsync(It.Is<UpdateItemRequest>(r => r.IsRequestEquivalentTo(request))))
            .ThrowsAsync(ex);
    }

    public void SetupUpdateItemWithTypesAsync(UpdateItemWithTypesRequest request)
    {
        this.SetupInOrder(m => m.UpdateItemWithTypesAsync(
                It.Is<UpdateItemWithTypesRequest>(r => r.IsRequestEquivalentTo(request))))
            .Returns(Task.CompletedTask);
    }

    public void SetupUpdateItemWithTypesAsyncThrowing(UpdateItemWithTypesRequest request, Exception ex)
    {
        this.SetupInOrder(m => m.UpdateItemWithTypesAsync(
                It.Is<UpdateItemWithTypesRequest>(r => r.IsRequestEquivalentTo(request))))
            .ThrowsAsync(ex);
    }

    public void SetupModifyItemAsync(ModifyItemRequest request)
    {
        this.SetupInOrder(m => m.ModifyItemAsync(It.Is<ModifyItemRequest>(r => r.IsRequestEquivalentTo(request))))
            .Returns(Task.CompletedTask);
    }

    public void SetupModifyItemAsyncThrowing(ModifyItemRequest request, Exception ex)
    {
        this.SetupInOrder(m => m.ModifyItemAsync(It.Is<ModifyItemRequest>(r => r.IsRequestEquivalentTo(request))))
            .ThrowsAsync(ex);
    }

    public void SetupModifyItemWithTypesAsync(ModifyItemWithTypesRequest request)
    {
        this.SetupInOrder(m => m.ModifyItemWithTypesAsync(
                It.Is<ModifyItemWithTypesRequest>(r => r.IsRequestEquivalentTo(request))))
            .Returns(Task.CompletedTask);
    }

    public void SetupModifyItemWithTypesAsyncThrowing(ModifyItemWithTypesRequest request, Exception ex)
    {
        this.SetupInOrder(m => m.ModifyItemWithTypesAsync(
                It.Is<ModifyItemWithTypesRequest>(r => r.IsRequestEquivalentTo(request))))
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

    public void SetupDeleteItemAsync(DeleteItemRequest request)
    {
        this.SetupInOrder(m => m.DeleteItemAsync(
                It.Is<DeleteItemRequest>(r => r.IsRequestEquivalentTo(request))))
            .Returns(Task.CompletedTask);
    }

    public void SetupDeleteItemAsyncThrowing(DeleteItemRequest request, Exception ex)
    {
        this.SetupInOrder(m => m.DeleteItemAsync(
                It.Is<DeleteItemRequest>(r => r.IsRequestEquivalentTo(request))))
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
        this.SetupInOrder(m => m.CreateStoreAsync(store)).Returns(Task.CompletedTask);
    }

    public void SetupCreateStoreAsyncThrowing(EditedStore store, Exception ex)
    {
        this.SetupInOrder(m => m.CreateStoreAsync(store)).ThrowsAsync(ex);
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
}