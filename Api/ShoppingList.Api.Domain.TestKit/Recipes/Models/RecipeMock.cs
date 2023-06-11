using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;

public class RecipeMock : Mock<IRecipe>
{
    public RecipeMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupModifyAsync(RecipeModification modification, IValidator validator)
    {
        Setup(m => m.ModifyAsync(modification, validator))
            .Returns(Task.CompletedTask);
    }

    public void VerifyModifyAsync(RecipeModification modification, IValidator validator, Func<Times> times)
    {
        Verify(m => m.ModifyAsync(modification, validator), times);
    }

    public void SetupModifyIngredientsAfterItemUpdate(ItemId oldItemId, IItem newItem)
    {
        Setup(m => m.ModifyIngredientsAfterItemUpdate(oldItemId, newItem));
    }

    public void VerifyModifyIngredientsAfterItemUpdate(ItemId oldItemId, IItem newItem, Func<Times> times)
    {
        Verify(m => m.ModifyIngredientsAfterItemUpdate(oldItemId, newItem), times);
    }

    public void SetupRemoveDefaultItem(ItemId itemId, ItemTypeId? itemTypeId)
    {
        Setup(m => m.RemoveDefaultItem(itemId, itemTypeId));
    }

    public void VerifyRemoveDefaultItem(ItemId itemId, ItemTypeId? itemTypeId, Func<Times> times)
    {
        Verify(m => m.RemoveDefaultItem(itemId, itemTypeId), times);
    }

    public void SetupModifyIngredientsAfterAvailabilityWasDeleted(ItemId itemId, ItemTypeId? itemTypeId, IItem item,
        StoreId storeId)
    {
        Setup(m => m.ModifyIngredientsAfterAvailabilityWasDeleted(itemId, itemTypeId, item, storeId));
    }

    public void VerifyModifyIngredientsAfterAvailabilityWasDeleted(ItemId itemId, ItemTypeId? itemTypeId, IItem item,
        StoreId storeId, Func<Times> times)
    {
        Verify(m => m.ModifyIngredientsAfterAvailabilityWasDeleted(itemId, itemTypeId, item, storeId), times);
    }
}