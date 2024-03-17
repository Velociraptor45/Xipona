using Moq;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Items.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Shared.Services;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Items.Effects;

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