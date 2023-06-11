using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Services.Modifications;

public class RecipeModificationServiceMock : Mock<IRecipeModificationService>
{
    public RecipeModificationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupModifyAsync(RecipeModification modification)
    {
        Setup(m => m.ModifyAsync(modification))
            .Returns(Task.CompletedTask);
    }

    public void VerifyModifyAsync(RecipeModification modification, Func<Times> times)
    {
        Verify(m => m.ModifyAsync(modification), times);
    }

    public void SetupModifyIngredientsAfterItemUpdateAsync(ItemId oldItemId, IItem newItem)
    {
        Setup(m => m.ModifyIngredientsAfterItemUpdateAsync(oldItemId, newItem))
            .Returns(Task.CompletedTask);
    }

    public void VerifyModifyIngredientsAfterItemUpdateAsync(ItemId oldItemId, IItem newItem, Func<Times> times)
    {
        Verify(m => m.ModifyIngredientsAfterItemUpdateAsync(oldItemId, newItem), times);
    }

    public void SetupModifyIngredientsAfterAvailabilityWasDeletedAsync(ItemId itemId, ItemTypeId? itemTypeId,
        StoreId storeId)
    {
        Setup(m => m.ModifyIngredientsAfterAvailabilityWasDeletedAsync(itemId, itemTypeId, storeId))
            .Returns(Task.CompletedTask);
    }

    public void VerifyModifyIngredientsAfterAvailabilityWasDeletedAsync(ItemId itemId, ItemTypeId? itemTypeId,
        StoreId storeId, Func<Times> times)
    {
        Verify(m => m.ModifyIngredientsAfterAvailabilityWasDeletedAsync(itemId, itemTypeId, storeId), times);
    }

    public void SetupRemoveDefaultItemAsync(ItemId itemId, ItemTypeId itemTypeId)
    {
        Setup(m => m.RemoveDefaultItemAsync(itemId, itemTypeId))
            .Returns(Task.CompletedTask);
    }

    public void VerifyRemoveDefaultItemAsync(ItemId itemId, ItemTypeId itemTypeId, Func<Times> times)
    {
        Verify(m => m.RemoveDefaultItemAsync(itemId, itemTypeId), times);
    }
}