using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;

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
}