using Moq;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Services;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.ShoppingList.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.ShoppingLists.Effects;

public class ShoppingListEffectsFixtureBase : EffectsFixtureBase
{
    protected readonly ShoppingListStateMock ShoppingListStateMock = new(MockBehavior.Strict);
    protected readonly NavigationManagerMock NavigationManagerMock = new(MockBehavior.Strict);
    protected ShoppingListState State = new DomainTestBuilder<ShoppingListState>().Create();

    public void SetupStateReturningState()
    {
        ShoppingListStateMock.SetupValue(State);
    }
}