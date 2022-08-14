using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Services.Queries;

public class RecipeQueryServiceMock : Mock<IRecipeQueryService>
{
    public RecipeQueryServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupSearchByNameAsync(string searchInput, IEnumerable<RecipeSearchResult> returnValue)
    {
        Setup(m => m.SearchByNameAsync(searchInput)).ReturnsAsync(returnValue);
    }
}