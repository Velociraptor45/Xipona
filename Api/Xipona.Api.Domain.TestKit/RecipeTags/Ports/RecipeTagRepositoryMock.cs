using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Ports;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.RecipeTags.Ports;

public class RecipeTagRepositoryMock : Mock<IRecipeTagRepository>
{
    public RecipeTagRepositoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupFindAllAsync(IEnumerable<IRecipeTag> returnValue)
    {
        Setup(x => x.FindAllAsync()).ReturnsAsync(returnValue);
    }

    public void SetupStoreAsync(IRecipeTag recipeTag, IRecipeTag returnValue)
    {
        Setup(x => x.StoreAsync(recipeTag)).ReturnsAsync(returnValue);
    }
}