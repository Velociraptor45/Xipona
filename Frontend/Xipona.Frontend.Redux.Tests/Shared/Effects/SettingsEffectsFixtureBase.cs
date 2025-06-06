using Moq;
using Xipona.Frontend.Redux.Shared.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.Shared.Ports;
using Xipona.Frontend.Redux.TestKit.Shared.States;

namespace Xipona.Frontend.Redux.Tests.Shared.Effects;

public class SettingsEffectsFixtureBase : EffectsFixtureBase
{
    protected readonly SharedStateMock SharedStateMock = new(MockBehavior.Strict);
    protected readonly ShoppingListNotificationServiceMock NotificationServiceMock = new(MockBehavior.Strict);
    protected SharedState State = new DomainTestBuilder<SharedState>().Create();

    public void SetupStateReturningState()
    {
        SharedStateMock.SetupValue(State);
    }
}
