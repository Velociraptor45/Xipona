using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

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

    public void SetupSearchByAsync(string searchInput, IEnumerable<RecipeSearchResult> expectedResult)
    {
        Setup(m => m.SearchByAsync(searchInput)).ReturnsAsync(expectedResult);
    }

    public void SetupFindByAsync(RecipeId recipeId, IRecipe? returnValue)
    {
        Setup(m => m.FindByAsync(recipeId)).ReturnsAsync(returnValue);
    }
}