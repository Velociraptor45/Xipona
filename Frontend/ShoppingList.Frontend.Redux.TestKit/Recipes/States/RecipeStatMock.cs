using Fluxor;
using Moq;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Recipes.States;

public class RecipeStatMock : Mock<IState<RecipeState>>
{
    public RecipeStatMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupValue(RecipeState returnValue)
    {
        Setup(x => x.Value).Returns(returnValue);
    }
}