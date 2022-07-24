using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Ports;

public class RecipeRepositoryMock : Mock<IRecipeRepository>
{
    public RecipeRepositoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupStoreAsync(IRecipe recipe, IRecipe returnValue)
    {
        Setup(m => m.StoreAsync(recipe))
            .ReturnsAsync(returnValue);
    }

    public void VerifyStoreAsync(IRecipe recipe, Func<Times> times)
    {
        Verify(m => m.StoreAsync(recipe), times);
    }
}