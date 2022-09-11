using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

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
}