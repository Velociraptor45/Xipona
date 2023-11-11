using Moq;
using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Manufacturers.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Services;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Manufacturers.Effects;

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