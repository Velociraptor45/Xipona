using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.RecipeTags.Ports;

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