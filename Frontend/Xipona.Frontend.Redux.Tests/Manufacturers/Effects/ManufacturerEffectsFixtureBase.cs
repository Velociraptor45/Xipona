using Moq;
using ProjectHermes.Xipona.Frontend.Redux.Manufacturers.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Manufacturers.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Shared.Services;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Manufacturers.Effects;

public class ManufacturerEffectsFixtureBase : EffectsFixtureBase
{
    protected readonly ManufacturerStateMock ManufacturerStateMock = new(MockBehavior.Strict);
    protected readonly NavigationManagerMock NavigationManagerMock = new(MockBehavior.Strict);
    protected ManufacturerState State = new DomainTestBuilder<ManufacturerState>().Create();

    public void SetupStateReturningState()
    {
        ManufacturerStateMock.SetupValue(State);
    }
}