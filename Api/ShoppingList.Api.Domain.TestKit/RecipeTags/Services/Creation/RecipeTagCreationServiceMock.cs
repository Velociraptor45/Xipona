using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Services.Creation;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.RecipeTags.Services.Creation;

public class RecipeTagCreationServiceMock : Mock<IRecipeTagCreationService>
{
    public RecipeTagCreationServiceMock(MockBehavior mockBehavior) : base(mockBehavior)
    {
    }

    public void SetupCreateAsync(string name, IRecipeTag returnValue)
    {
        Setup(x => x.CreateAsync(name)).ReturnsAsync(returnValue);
    }

    public void VerifyCreateAsync(string name, Func<Times> times)
    {
        Verify(x => x.CreateAsync(name), times);
    }
}