using Fluxor;
using Moq;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;

namespace ProjectHermes.Xipona.Frontend.Redux.TestKit.Recipes.States;

public class RecipeStateMock : Mock<IState<RecipeState>>
{
    public RecipeStateMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupValue(RecipeState returnValue)
    {
        Setup(x => x.Value).Returns(returnValue);
    }
}