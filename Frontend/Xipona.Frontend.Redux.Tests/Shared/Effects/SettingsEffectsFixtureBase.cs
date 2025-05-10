using Moq;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Shared.Ports;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Shared.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Shared.Effects;

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
