using Moq;
using Xipona.Frontend.Redux.Items.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.Items.States;
using Xipona.Frontend.Redux.TestKit.Shared.Services;

namespace Xipona.Frontend.Redux.Tests.Items.Effects;

public class ItemEffectsFixtureBase : EffectsFixtureBase
{
    protected readonly ItemStateMock ItemStateMock = new(MockBehavior.Strict);
    protected readonly NavigationManagerMock NavigationManagerMock = new(MockBehavior.Strict);
    protected ItemState State = new DomainTestBuilder<ItemState>().Create();

    public ItemEffectsFixtureBase()
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
        ItemStateMock.SetupValue(State);
    }
}