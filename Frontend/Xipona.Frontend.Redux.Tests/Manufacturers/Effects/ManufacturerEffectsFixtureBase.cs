using Moq;
using Xipona.Frontend.Redux.Manufacturers.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.Manufacturers.States;
using Xipona.Frontend.Redux.TestKit.Shared.Services;

namespace Xipona.Frontend.Redux.Tests.Manufacturers.Effects;

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