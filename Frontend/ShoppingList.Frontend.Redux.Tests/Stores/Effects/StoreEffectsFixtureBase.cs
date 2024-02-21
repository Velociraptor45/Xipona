using Moq;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Services;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Stores.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Stores.Effects;

public class StoreEffectsFixtureBase : EffectsFixtureBase
{
    protected readonly StoreStateMock StoreStateMock = new(MockBehavior.Strict);
    protected readonly NavigationManagerMock NavigationManagerMock = new(MockBehavior.Strict);
    protected StoreState State = new DomainTestBuilder<StoreState>().Create();

    public StoreEffectsFixtureBase()
    {
        State = State with
        {
            Editor = State.Editor with
            {
                ValidationResult = new EditorValidationResult()
            }
        };
    }

    public void SetupStateReturningState()
    {
        StoreStateMock.SetupValue(State);
    }
}