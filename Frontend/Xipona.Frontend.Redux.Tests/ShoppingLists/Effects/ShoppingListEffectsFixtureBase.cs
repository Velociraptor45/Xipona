using Moq;
using Xipona.Frontend.Redux.ShoppingList.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.Shared.Services;
using Xipona.Frontend.Redux.TestKit.ShoppingList.States;

namespace Xipona.Frontend.Redux.Tests.ShoppingLists.Effects;

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