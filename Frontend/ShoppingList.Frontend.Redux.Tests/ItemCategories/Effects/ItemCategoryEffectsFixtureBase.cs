using Moq;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Services;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.ItemCategories.Effects;

public class ItemCategoryEffectsFixtureBase : EffectsFixtureBase
{
    protected readonly ItemCategoryStateMock ItemCategoryStateMock = new(MockBehavior.Strict);
    protected readonly NavigationManagerMock NavigationManagerMock = new(MockBehavior.Strict);
    protected ItemCategoryState State = new DomainTestBuilder<ItemCategoryState>().Create();

    public void SetupStateReturningState()
    {
        ItemCategoryStateMock.SetupValue(State);
    }
}