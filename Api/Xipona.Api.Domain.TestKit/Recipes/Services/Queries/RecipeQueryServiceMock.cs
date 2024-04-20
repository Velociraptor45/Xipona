using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Services.Queries;

public class RecipeQueryServiceMock : Mock<IRecipeQueryService>
{
    public RecipeQueryServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupSearchByNameAsync(string searchInput, IEnumerable<RecipeSearchResult> returnValue)
    {
        Setup(m => m.SearchByNameAsync(searchInput)).ReturnsAsync(returnValue);
    }

    public void SetupGetAsync(RecipeId recipeId, RecipeReadModel returnValue)
    {
        Setup(m => m.GetAsync(recipeId)).ReturnsAsync(returnValue);
    }

    public void SetupSearchByTagIdsAsync(IEnumerable<RecipeTagId> tagIds, IEnumerable<RecipeSearchResult> returnValue)
    {
        Setup(m => m.SearchByTagIdsAsync(tagIds)).ReturnsAsync(returnValue);
    }
}