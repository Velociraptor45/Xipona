using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Services.Creations;

public class RecipeCreationServiceMock : Mock<IRecipeCreationService>
{
    public RecipeCreationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupCreateAsync(RecipeCreation creation, RecipeReadModel returnValue)
    {
        Setup(m => m.CreateAsync(creation))
            .ReturnsAsync(returnValue);
    }

    public void VerifyCreateAsync(RecipeCreation creation, Func<Times> times)
    {
        Verify(m => m.CreateAsync(creation), times);
    }
}