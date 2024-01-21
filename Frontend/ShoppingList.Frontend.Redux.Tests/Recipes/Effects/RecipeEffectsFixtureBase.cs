using Moq;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Services;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Recipes.Effects;

public class RecipeEffectsFixtureBase : EffectsFixtureBase
{
    protected readonly RecipeStateMock RecipeStateMock = new(MockBehavior.Strict);
    protected readonly NavigationManagerMock NavigationManagerMock = new(MockBehavior.Strict);
    protected RecipeState State = new DomainTestBuilder<RecipeState>().Create();

    public RecipeEffectsFixtureBase()
    {
        State = State with
        {
            Editor = State.Editor with
            {
                ValidationResult = new()
            }
        };
    }

    public void SetupStateReturningState()
    {
        RecipeStateMock.SetupValue(State);
    }
}