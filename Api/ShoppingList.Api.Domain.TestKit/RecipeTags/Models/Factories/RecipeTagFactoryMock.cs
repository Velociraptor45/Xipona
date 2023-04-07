using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models.Factories;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.RecipeTags.Models.Factories;

public class RecipeTagFactoryMock : Mock<IRecipeTagFactory>
{
    public RecipeTagFactoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupCreateNew(string name, IRecipeTag returnValue)
    {
        Setup(x => x.CreateNew(name)).Returns(returnValue);
    }
}