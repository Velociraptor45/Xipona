using Moq;
using ProjectHermes.Xipona.Frontend.Redux.Stores.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Shared.Services;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Stores.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Stores.Effects;

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