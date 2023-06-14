using Moq;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Services;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Items.Effects;

public class ItemEffectsFixtureBase : EffectsFixtureBase
{
    protected readonly ItemStateMock ItemStateMock = new(MockBehavior.Strict);
    protected readonly NavigationManagerMock NavigationManagerMock = new(MockBehavior.Strict);
    protected ItemState State = new DomainTestBuilder<ItemState>().Create();

    public void SetupStateReturningState()
    {
        ItemStateMock.SetupValue(State);
    }
}