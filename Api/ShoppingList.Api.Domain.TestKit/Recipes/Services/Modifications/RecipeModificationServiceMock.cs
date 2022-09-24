﻿using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;

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
}